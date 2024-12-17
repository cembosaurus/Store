using Business.Data;
using Business.Data.Tools.Interfaces;
using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Filters.Validation;
using Business.Http.Clients;
using Business.Http.Clients.Interfaces;
using Business.Identity.Enums;
using Business.Inventory.Http.Services;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Data;
using Business.Management.DI;
using Business.Metrics.DI;
using Business.Metrics.Http.Services;
using Business.Metrics.Http.Services.Interfaces;
using Business.Middlewares;
using Business.Scheduler.JWT;
using Business.Scheduler.JWT.Interfaces;
using Business.Tools;
using FluentValidation.AspNetCore;
using Identity.Data.Repositories;
using Identity.Data.Repositories.Interfaces;
using Identity.JWT;
using Identity.JWT.Interfaces;
using Identity.Middlewares;
using Identity.Models;
using Identity.Services;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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

ManagementService_DI.Register(builder);
MetricsService_DI.Register(builder);

builder.Services.AddSingleton<IExId, ExId>(); 
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();
builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<IdentityContext>();
builder.Services.AddSingleton<IJWTTokenStore, JWTTokenStore>();
builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IHttpCartService, HttpCartService>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddScoped<IJWT_Provider, JWT_Provider>();

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>().AddHttpMessageHandler<Metrics_HttpClientRequest_INTERCEPTOR>();



builder.Services.AddTransient<ConsoleWriter>();



builder.Services.AddIdentityCore<AppUser>(opt => {
    opt.Password.RequireNonAlphanumeric = false;    // Add more options to customize password complexity.
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
}).AddRoles<AppRole>()
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandler_MW>();

app.UseMiddleware<Metrics_MW>();

app.UseMiddleware<Identity_DbGuard_MW>();


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

Identity_DbGuard_MW.Migrate_Prep_Seed_DB(app);

GlobalConfig_Seed.Load(app);

app.Run();
