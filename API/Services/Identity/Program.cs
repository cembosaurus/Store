using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Filters.Validation;
using Business.Http;
using Business.Http.Interfaces;
using Business.Identity.Enums;
using Business.Inventory.Http.Services;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Data;
using Business.Management.Data.Interfaces;
using Business.Management.Http.Services;
using Business.Management.Http.Services.Interfaces;
using Business.Management.Services;
using Business.Management.Services.Interfaces;
using Business.Middlewares;
using Business.Scheduler.JWT;
using Business.Scheduler.JWT.Interfaces;
using FluentValidation.AspNetCore;
using Identity.Data;
using Identity.Data.Repositories;
using Identity.Data.Repositories.Interfaces;
using Identity.Models;
using Identity.Services;
using Identity.Services.Interfaces;
using Identity.Services.JWT;
using Identity.Services.JWT.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Identity.Data;
using Services.Identity.Data.Repositories;
using Services.Identity.Data.Repositories.Interfaces;
using Services.Identity.Models;
using System.Text;

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

builder.Services.AddSingleton<IAppsettings_DB, Appsettings_DB>();
builder.Services.AddScoped<IRemoteServicesInfo_Repo, RemoteServicesInfo_Repo>();
builder.Services.AddScoped<IRemoteServicesInfo_Provider, RemoteServicesInfo_Provider>();
builder.Services.AddScoped<IHttpManagementService, HttpManagementService>();
builder.Services.AddTransient<IAppsettingsService, AppsettingsService>();
//builder.Services.AddScoped<IHttpApiKeyAuthService, HttpApiKeyAuthService>();
builder.Services.AddSingleton<IExId, ExId>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer(builder.Configuration.GetSection("Congif.Local:ConnectionStrings:IdentityConnStr").Value));
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

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>();


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
                    var secret = builder.Configuration.GetSection("Config.Global:Auth:JWTKey").Value;
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

app.UseMiddleware<Metrics_MW>();

// Custom Exception Handler:
app.UseMiddleware<ErrorHandler_MW>();

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

// DB:
try
{
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
}
catch (SqlException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;

    switch (ex.Number)
    {
        case 11001:
            Console.WriteLine($"\n\n\n--> DB Connection failed ! Reason: {ex.Message}\n\n\n");
            break;
        default:
            Console.WriteLine($"\n\n\n--> SQL Error ! Reason: {ex.Message}\n\n\n");
            break;
    }

    Console.ResetColor();
}


app.Run();
