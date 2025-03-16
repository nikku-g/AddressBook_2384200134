using AutoMapper;
using BCrypt.Net;
using BusinessLayer.Interface;
using BusinessLayer.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Event;
using ModelLayer.Model;
using ReposatoryLayer.Entity;
using ReposatoryLayer.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayer.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAddressBookRL _addressBookRL;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(IAddressBookRL addressBookRL, IMapper mapper, IConfiguration configuration, IEmailService emailService)
        {
            _addressBookRL = addressBookRL;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
        }

        public void RegisterUser(UserDTO userDto)
        {
            // Registration logic
            var userRegisteredEvent = new UserRegisteredEvent
            {
                Email = userDto.Email,
                Name = userDto.Username
            };

            var publisher = new RabbitMQPublisher();
            publisher.Publish(userRegisteredEvent, "user.registered");
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var userEntity = _addressBookRL.GetAddresse().FirstOrDefault(x => x.Email == email);
            if (userEntity == null)
                throw new Exception("User not found.");

            var resetToken = GenerateResetToken(userEntity);

            // Send reset password email with the token
            await _emailService.SendResetPasswordEmail(userEntity.Email, resetToken);

            return "Password reset email sent.";
        }

        public async Task<string> ResetPasswordAsync(string token, string newPassword)
        {
            // Validate the reset token
            var userId = ValidateResetToken(token);
            if (userId == 0)
                throw new Exception("Invalid or expired token.");

            var userEntity = _addressBookRL.GetAddresse().FirstOrDefault(x => x.Id == userId);
            if (userEntity == null)
                throw new Exception("User not found.");

            // Hash the new password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            userEntity.PasswordHash = passwordHash;

            // Update password in the database
            _addressBookRL.UpdateAddress(userEntity);

            return "Password has been reset successfully.";
        }

        private string GenerateResetToken(UserEntity user)
        {
            // Generate a reset token for password reset
            var claims = new[]
            {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),  // Set expiry for the reset token
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private int ValidateResetToken(string token)
        {
            // Validate the reset token and extract userId
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jsonToken == null)
                return 0;

            var userIdClaim = jsonToken?.Claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return 0;

            return int.Parse(userIdClaim.Value);  // Return the userId extracted from the token
        }
        public async Task<UserDTO> RegisterAsync(UserRegistrationDTO userRegistrationDTO)
        {
            var existingUser = _addressBookRL.GetAddresse().FirstOrDefault(x => x.Username == userRegistrationDTO.Username);
            if (existingUser != null) throw new Exception("Username already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegistrationDTO.Password);
            var userEntity = new UserEntity
            {
                Username = userRegistrationDTO.Username,
                Email = userRegistrationDTO.Email,
                PasswordHash = passwordHash
            };

            // Save the new user to the database (you may add a save method in your repository)
            var createdUser = _addressBookRL.AddAddress(userEntity); // This is your existing method for adding entities

            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<string> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var userEntity = _addressBookRL.GetAddresse().FirstOrDefault(x => x.Username == userLoginDTO.Username);

            if (userEntity == null || !BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, userEntity.PasswordHash))
                throw new Exception("Invalid username or password");

            var token = GenerateJwtToken(userEntity);
            return token;
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var claims = new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Username),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
