using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using ReposatoryLayer.Context;
using ReposatoryLayer.Interface;
using ReposatoryLayer.Service;

var builder = WebApplication.CreateBuilder(args);

// Register the DbContext with the DI container
builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();
builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");  // Make sure this key matches the key in appsettings.json
builder.Services.AddDbContext<AddressContext>(options => options.UseSqlServer(connectionString));

// Register other services, controllers, etc.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
