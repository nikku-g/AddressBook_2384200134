using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IAuthService
    {
        Task<UserDTO> RegisterAsync(UserRegistrationDTO userRegistrationDTO);
        Task<string> LoginAsync(UserLoginDTO userLoginDTO);
        Task<string> ForgotPasswordAsync(string email);
        Task<string> ResetPasswordAsync(string token, string newPassword);
    }
}
