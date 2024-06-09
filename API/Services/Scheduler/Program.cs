using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Filters.Validation;
using Business.Http;
using Business.Http.Interfaces;
using Business.Identity.Enums;
using Business.Identity.Http.Services;
using Business.Identity.Http.Services.Interfaces;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Scheduler.Data;
using Scheduler.Data.Repositories;
using Scheduler.Data.Repositories.Interfaces;
using Scheduler.Modules;
using Scheduler.Services;
using Scheduler.Services.Interfaces;
using Scheduler.Startup;
using Scheduler.Tasks;
using Scheduler.Tasks.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidationFilter>();
});

builder.Services.AddSingleton<IAppsettings_DB, Appsettings_DB>();
builder.Services.AddScoped<IRemoteServicesInfo_Repo, RemoteServicesInfo_Repo>();
builder.Services.AddScoped<IRemoteServicesInfo_Provider, RemoteServicesInfo_Provider>();
builder.Services.AddScoped<IHttpManagementService, HttpManagementService>();
builder.Services.AddTransient<IAppsettingsService, AppsettingsService>();
builder.Services.AddSingleton<IExId, ExId>();

builder.Services.AddFluentValidation(conf => {
    conf.DisableDataAnnotationsValidation = true;
    //conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);                    // scans for validations in this poroject
    conf.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());     // scans for validations in all linked projects or libraries
    conf.AutomaticValidationEnabled = true;
});

// Scheduler modules (CartItemLocker... etc):
builder.Services.RegisterSchedulerTasks(builder.Configuration);

builder.Services.AddTransient<IRunAtStartup, RunAtStartup>();
builder.Services.AddDbContext<SchedulerDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SchedulerConnStr")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
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

builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddScoped<IHttpIdentityService, HttpIdentityService>();
builder.Services.AddScoped<IHttpCartService, HttpCartService>();
builder.Services.AddScoped<IHttpApiKeyAuthService, HttpApiKeyAuthService>();

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>();


// Middleware that authenticate request before hitting controller (endpoint):
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    var secret = builder.Configuration.GetSection("Auth:JWTKey").Value;
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<Metrics_MW>();

// Custom Exception Handler:
app.UseMiddleware<ErrorHandler_MW>();


//app.Use(async (context, next) => 
//{
//    Console.WriteLine($".... FIRST middleware BEFORE .....Req: {context.Request.Path} -- Resp: {context.Response.StatusCode}");
//    await next.Invoke(context);
//    Console.WriteLine($".... FIRST middleware AFTER .....Req: {context.Request.Path} -- Resp: {context.Response.StatusCode}");

//});


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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


// ApiKey Auth:
app.UseMiddleware<ApiKeyAuth_MW>();


using (var scope = app.Services.CreateScope())
{
    var runAtStartup = scope.ServiceProvider.GetRequiredService<IRunAtStartup>();

    await runAtStartup.Run();
}

app.Run();