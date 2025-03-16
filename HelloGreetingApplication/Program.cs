using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ReposatoryLayer.Context;
using ReposatoryLayer.Interface;
using ReposatoryLayer.Service;
using FluentValidation.AspNetCore;
using RabbitMQ.Client;
using ModelLayer.Validators;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text; // Required for FluentValidation

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var config = builder.Configuration.GetSection("RabbitMQ");
    var factory = new ConnectionFactory()
    {
        HostName = config["HostName"],
        Port = int.Parse(config["Port"]),
        UserName = config["Username"],
        Password = config["Password"]
    };
    return factory;
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"]; // Example: localhost:6379
    options.InstanceName = "AddressBookApp:";
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();

// Register the DbContext with the DI container
builder.Services.AddScoped<IAddressBookService, AddressBookService>();
builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();
builder.Services.AddScoped<IEmailService, EmailService>();
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");  // Make sure this key matches the key in appsettings.json
builder.Services.AddDbContext<AddressContext>(options => options.UseSqlServer(connectionString));

// Register controllers (important to add first before FluentValidation)
builder.Services.AddControllers();

// Register FluentValidation with the correct assembly containing the validators
builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<RequestModelValidator>());

// Register AutoMapper (ensure you have an AddressMapping profile)
builder.Services.AddAutoMapper(typeof(AddressMapping));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();