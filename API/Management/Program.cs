using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
using Business.Identity.Enums;
using Business.Identity.Http.Services;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;
using Business.Management.Data.Interfaces;
using Business.Management.Http.Services;
using Business.Management.Http.Services.Interfaces;
using Business.Management.Services;
using Business.Management.Services.Interfaces;
using Business.Middlewares;
using Business.Scheduler.JWT;
using Business.Scheduler.JWT.Interfaces;
using Management.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHostedService<Management_Worker>();

builder.Services.AddSingleton<IAppsettings_DB, Appsettings_DB>();
builder.Services.AddScoped<IRemoteServicesInfo_Repo, RemoteServicesInfo_Repo>();
builder.Services.AddScoped<IRemoteServicesInfo_Provider, RemoteServicesInfo_Provider>();

builder.Services.AddSingleton<IExId, ExId>();
builder.Services.AddSingleton<FileSystemWatcher>();
builder.Services.AddSingleton<IAppsettingsService, AppsettingsService>();
builder.Services.AddSingleton<IJWTTokenStore, JWTTokenStore>();

builder.Services.AddScoped<IHttpManagementService, HttpManagementService>();
builder.Services.AddScoped<IHttpApiKeyAuthService, HttpApiKeyAuthService>();
builder.Services.AddScoped<IHttpIdentityService, HttpIdentityService>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.Configure<Config_Global_Model_AS>(builder.Configuration.GetSection("Config.Global"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IServiceResultFactory, ServiceResultFactory>();

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>();


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


builder.Services.AddSwaggerGen();



var app = builder.Build();


app.UseMiddleware<Metrics_MW>();

app.UseMiddleware<ServiceId_MW>();

// Custom Exception Handler:
app.UseMiddleware<ErrorHandler_MW>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.MapGet("/", () => "Zedous !");

app.MapControllers();

app.Run();
