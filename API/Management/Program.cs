using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Clients.Interfaces;
using Business.Http.Services;
using Business.Http.Services.Interfaces;
using Business.Identity.Enums;
using Business.Identity.Http.Services;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings;
using Business.Management.Http.Services;
using Business.Management.Http.Services.Interfaces;
using Business.Metrics.Http.Services;
using Business.Metrics.Http.Services.Interfaces;
using Business.Middlewares;
using Business.Scheduler.JWT;
using Business.Scheduler.JWT.Interfaces;
using Management.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Business.Management.Data;
using Business.Management.Services.Interfaces;
using Business.Management.Services;
using Business.Management.Appsettings.Models;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHostedService<Management_Worker>();

builder.Services.AddSingleton<Config_Global_DB>();
builder.Services.AddScoped<IConfig_Global_REPO, Config_Global_REPO>();
builder.Services.AddTransient<IAppsettings_PROVIDER, Appsettings_PROVIDER>();
builder.Services.AddScoped<IGlobalConfig_PROVIDER, GlobalConfig_PROVIDER>();
builder.Services.Configure<Config_Global_AS_MODEL>(builder.Configuration.GetSection("Config.Global"));

builder.Services.AddScoped<IHttpAllServices, HttpAllServices>();

builder.Services.AddSingleton<IExId, ExId>(); 
builder.Services.AddSingleton<FileSystemWatcher>();
builder.Services.AddSingleton<IJWTTokenStore, JWTTokenStore>();

builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();

builder.Services.AddScoped<IHttpManagementService, HttpManagementService>();
builder.Services.AddScoped<IHttpIdentityService, HttpIdentityService>();
builder.Services.AddHttpContextAccessor();

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
                        ValidateIssuer = false,     // BE - API
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

app.UseMiddleware<ErrorHandler_MW>();

app.UseMiddleware<Metrics_MW>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.MapGet("/", () => "Zedous !");

app.MapControllers();

app.Run();
