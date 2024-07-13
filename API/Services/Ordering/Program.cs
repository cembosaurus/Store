using Business.Data;
using Business.Data.Tools.Interfaces;
using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Filters.Validation;
using Business.Http.Clients;
using Business.Http.Clients.Interfaces;
using Business.Identity.Enums;
using Business.Identity.Http.Services;
using Business.Identity.Http.Services.Interfaces;
using Business.Inventory.Http.Services;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;
using Business.Management.Http.Services;
using Business.Management.Http.Services.Interfaces;
using Business.Management.Services;
using Business.Management.Services.Interfaces;
using Business.Middlewares;
using Business.Payment.Http.Services;
using Business.Payment.Http.Services.Interfaces;
using Business.Scheduler.Http.Services;
using Business.Scheduler.Http.Services.Interfaces;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ordering.Data;
using Ordering.Data.Repositories;
using Ordering.Data.Repositories.Interfaces;
using Ordering.Middlewares;
using Ordering.Services;
using Ordering.Services.Interfaces;
using Ordering.Tools;
using Ordering.Tools.Interfaces;
using System.ComponentModel;
using System.Reflection;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidationFilter>();
});

builder.Services.AddSingleton<Config_Global_DB>();
builder.Services.AddScoped<IConfig_Global_REPO, Config_Global_REPO>();
builder.Services.AddScoped<IGlobalConfig_PROVIDER, GlobalConfig_PROVIDER>();
builder.Services.AddScoped<IHttpManagementService, HttpManagementService>();
builder.Services.AddTransient<IAppsettings_PROVIDER, Appsettings_PROVIDER>();
builder.Services.AddSingleton<IExId, ExId>(); 
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();
builder.Services.Configure<Config_Global_AS_MODEL>(builder.Configuration.GetSection("Config.Global"));

builder.Services.AddFluentValidation(conf => {
    conf.DisableDataAnnotationsValidation = true;
    //conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);                    // scans for validations in this poroject
    conf.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());     // scans for validations in all linked projects or libraries
    conf.AutomaticValidationEnabled = true;
});

builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });         // Allow optional argument in controller's action

builder.Services.AddDbContext<OrderingContext>(opt => opt.UseSqlServer(builder.Configuration.GetSection("Config.Local:ConnectionStrings:OrderingConnStr").Value, opt => opt.EnableRetryOnFailure()));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IArchiveService, ArchiveService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IHttpItemService, HttpItemService>();
builder.Services.AddScoped<IHttpCatalogueItemService, HttpCatalogueItemService>();
builder.Services.AddScoped<IHttpItemPriceService, HttpItemPriceService>();
builder.Services.AddScoped<IHttpAddressService, HttpAddressService>();
builder.Services.AddScoped<IHttpPaymentService, HttpPaymentService>();
builder.Services.AddScoped<IHttpSchedulerService, HttpSchedulerService>();

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemsRepository, CartItemsRepository>();
builder.Services.AddScoped<IArchiveRepository , ArchiveRepository>();

builder.Services.AddScoped<ICart, OrderingTools>();
builder.Services.AddScoped<IOrder, OrderingTools>();
builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<Metrics_Client_MW>();

app.UseMiddleware<AppId_MW>();

app.UseMiddleware<Ordering_DbGuard_MW>();

app.UseMiddleware<ErrorHandler_MW>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

Ordering_DbGuard_MW.Migrate_DB(app);

app.Run();
