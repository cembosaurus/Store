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
using Business.Management.Data;
using Business.Management.DI;
using Business.Metrics.Http.Clients;
using Business.Metrics.Http.Clients.Interfaces;
using Business.Metrics.Http.Services;
using Business.Metrics.Http.Services.Interfaces;
using Business.Middlewares;
using Business.Scheduler.JWT;
using Business.Scheduler.JWT.Interfaces;
using Business.Tools;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scheduler.Data;
using Scheduler.Data.Repositories;
using Scheduler.Data.Repositories.Interfaces;
using Scheduler.Middlewares;
using Scheduler.Modules;
using Scheduler.Services;
using Scheduler.Services.Interfaces;
using Scheduler.StartUp;
using Scheduler.StartUp.Interfaces;
using Scheduler.Tasks;
using Scheduler.Tasks.Interfaces;
using System.Text;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidationFilter>();
});

ManagementService_DI.Register(builder);

builder.Services.AddSingleton<IExId, ExId>(); 
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();
builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();

builder.Services.AddFluentValidation(conf => {
    conf.DisableDataAnnotationsValidation = true;
    //conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);                    // scans for validations in this poroject
    conf.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());     // scans for validations in all linked projects or libraries
    conf.AutomaticValidationEnabled = true;
});

// Scheduler modules (CartItemLocker... etc):
builder.Services.RegisterSchedulerTasks(builder.Configuration);
builder.Services.AddDbContext<SchedulerDBContext>();
builder.Services.AddTransient<IRunAtStartUp, RunAtStartUp>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IJWTTokenStore, JWTTokenStore>();
builder.Services.AddTransient<ICartItemLocker, CartItemLocker>();

builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddScoped<ICartItemLockRepository, CartItemLockRepository>();

// Multiple interfaces in one service:
builder.Services.AddScoped<OrderingService>();
builder.Services.AddScoped<IArchiveService>(sp => sp.GetService<OrderingService>());
builder.Services.AddScoped<ICartItemsService>(sp => sp.GetService<OrderingService>());
builder.Services.AddScoped<ICartService>(sp => sp.GetService<OrderingService>());
builder.Services.AddScoped<IOrderService>(sp => sp.GetService<OrderingService>());

builder.Services.AddScoped<IHttpIdentityService, HttpIdentityService>();
builder.Services.AddScoped<IHttpCatalogueItemService, HttpCatalogueItemService>();
builder.Services.AddScoped<IHttpCartService, HttpCartService>();

//builder.Services.AddScoped<Metrics_HttpRequestHandler>();
//builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>().AddHttpMessageHandler<Metrics_HttpRequestHandler>();
builder.Services.AddHttpClient<IHttpClient_Metrics, HttpClient_Metrics>();
builder.Services.AddScoped<IHttpAppClient, HttpAppClient>();


builder.Services.AddTransient<ConsoleWriter>();


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

app.UseMiddleware<ErrorHandler_MW>();

app.UseMiddleware<Metrics_MW>();

app.UseMiddleware<Scheduler_DbGuard_MW>();


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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

Scheduler_DbGuard_MW.HandleExpiredItems(app);

GlobalConfig_Seed.Load(app);

app.Run();