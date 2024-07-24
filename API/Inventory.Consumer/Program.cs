using Business.Filters.Validation;
using Business.Identity.Enums;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Metrics.Http.Services.Interfaces;
using Business.Metrics.Http.Services;
using Business.Middlewares;
using Inventory.Consumer.AMQPServices;
using Inventory.Consumer.Data;
using Inventory.Consumer.Data.Repositories;
using Inventory.Consumer.Data.Repositories.Interfaces;
using Inventory.Consumer.Services;
using Inventory.Consumer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Business.Http.Clients.Interfaces;
using Business.Http.Clients;
using Business.Management.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidationFilter>();
});

// Add RABBIT_MQ listener / subscriber:
// ... register MessageBusSubscriber as Singleton for DI access:
builder.Services.AddSingleton<MessageBusSubscriber>();
// ... deploy MessageBusSubscriber singleton service as a Background Service for AMQP listenning:
builder.Services.AddHostedService(provider => provider.GetService<MessageBusSubscriber>());

builder.Services.AddDbContext<InventoryContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

Management_Register.Register(builder);

builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();
builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>();

builder.Services.AddSingleton<IItemRepository, ItemRepository>();
builder.Services.AddSingleton<IItemService, ItemService>();
builder.Services.AddSingleton<IServiceResultFactory, ServiceResultFactory>();


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

PrepDB.PrepPopulation(app, app.Environment.IsProduction(), app.Configuration);

GlobalConfig_Seed.Load(app);

app.Run();
