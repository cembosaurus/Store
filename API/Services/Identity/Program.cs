using Business.Identity.Enums;
using Business.Libraries.Http.Interfaces;
using Business.Libraries.Http;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.Http;
using Business.Ordering.Http.Interfaces;
using Business.Scheduler.JWT;
using Business.Scheduler.JWT.Interfaces;
using Identity.Data;
using Identity.Data.Repositories;
using Identity.Data.Repositories.Interfaces;
using Identity.HttpServices;
using Identity.HttpServices.Interfaces;
using Identity.Models;
using Identity.Services;
using Identity.Services.Interfaces;
using Identity.Services.JWT;
using Identity.Services.JWT.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Identity.Data;
using Services.Identity.Data.Repositories;
using Services.Identity.Data.Repositories.Interfaces;
using Services.Identity.Models;
using System.Text;
using Business.Filters.Validation;
using FluentValidation.AspNetCore;
using Business.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidationFilter>();
});

builder.Services.AddFluentValidation(conf => {
    conf.DisableDataAnnotationsValidation = true;
    conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);                    // scans for validations in this poroject
    //conf.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());     // scans for validations in all linked projects or libraries
    conf.AutomaticValidationEnabled = true;
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnStr")));
builder.Services.AddSingleton<IJWTTokenStore, JWTTokenStore>();
builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IHttpCartService, HttpCartService>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddHttpClient<IHttpCartClient, HttpCartClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetSection("RemoteServices:OrderingService").Value);
});

builder.Services.AddIdentityCore<AppUser>(opt => {
    opt.Password.RequireNonAlphanumeric = false;    // Add more options to customize password complexity.
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
})
    .AddRoles<AppRole>()
    .AddRoleManager<RoleManager<AppRole>>()
    .AddRoleValidator<RoleValidator<AppRole>>()
    .AddSignInManager<SignInManager<AppUser>>()
    .AddEntityFrameworkStores<IdentityContext>();

// Middleware that authenticate request before hitting controller (endpoint):
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    var secret = builder.Configuration.GetSection("AppSettings:JWTKey").Value;
                    var secretByteArray = Encoding.ASCII.GetBytes(secret);

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretByteArray),
                        ValidateIssuer = false,     // BE - this app (server)
                        ValidateAudience = false    // FE - angular
                    };
                });

builder.Services.AddAuthorization(opt => {
    opt.AddPolicy(PolicyType.Administration.ToString(),
        p => p.RequireRole(
        RoleType.Admin.ToString()
    ));
    opt.AddPolicy(PolicyType.Management.ToString(),
        p => p.RequireRole(
        RoleType.Manager.ToString(),
        RoleType.Accountant.ToString(),
        RoleType.Seller.ToString()
    ));
    opt.AddPolicy(PolicyType.Support.ToString(),
        p => p.RequireRole(
        RoleType.ProductExpert.ToString()
    ));
    opt.AddPolicy(PolicyType.Shopping.ToString(),
        p => p.RequireRole(
        RoleType.Customer.ToString()
    ));
    opt.AddPolicy(PolicyType.Everyone.ToString(),
    p => p.RequireRole(
        RoleType.Admin.ToString(),
        RoleType.Manager.ToString(),
        RoleType.Accountant.ToString(),
        RoleType.Seller.ToString(),
        RoleType.ProductExpert.ToString(),
        RoleType.Customer.ToString(),
        RoleType.ServiceApp.ToString()
    ));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<Metrics>();

// Custom Exception Handler:
app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt => {
    opt.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

//app.UseHttpsRedirection();

// Run authentiction service:
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


// Migrations:
PrepDB.RunMigrations(app, app.Environment.IsProduction(), app.Configuration);

// Users and Roles:
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IIdentityService>();
    // Seed roples:
    await service.AddRoles();
    // Create admin and manager:
    await service.AddDefaultUsers();
}

// Seed DB:
PrepDB.PrepPopulation(app);



app.Run();
