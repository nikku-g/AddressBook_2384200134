using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using ReposatoryLayer.Context;
using ReposatoryLayer.Interface;
using ReposatoryLayer.Service;
using FluentValidation.AspNetCore;
using ModelLayer.Validators; // Required for FluentValidation

var builder = WebApplication.CreateBuilder(args);

// Register the DbContext with the DI container
builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();
builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");  // Make sure this key matches the key in appsettings.json
builder.Services.AddDbContext<AddressContext>(options => options.UseSqlServer(connectionString));

// Register controllers (important to add first before FluentValidation)
builder.Services.AddControllers();

// Register FluentValidation with the correct assembly containing the validators
builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<RequestModelValidator>());

// Register AutoMapper (ensure you have an AddressMapping profile)
builder.Services.AddAutoMapper(typeof(AddressMapping));

// Register other services
builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
