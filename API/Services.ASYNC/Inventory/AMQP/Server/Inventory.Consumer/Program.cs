using Business.Filters.Validation;
using Business.Http.Clients;
using Business.Http.Clients.Interfaces;
using Business.Identity.DI;
using Business.Identity.Enums;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Data;
using Business.Management.DI;
using Business.Management.Services;
using Business.Metrics.DI;
using Business.Metrics.Http.Services;
using Business.Metrics.Http.Services.Interfaces;
using Business.Middlewares;
using Business.Tools;
using Inventory.Consumer.AMQPServices;
using Inventory.Consumer.Data;
using Inventory.Consumer.Data.Repositories;
using Inventory.Consumer.Data.Repositories.Interfaces;
using Inventory.Consumer.Services;
using Inventory.Consumer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidationFilter>();
});

builder.Services.AddIdentityServiceIntegration(builder.Configuration);
builder.Services.AddManagementServiceIntegration(builder.Configuration);
builder.Services.AddMetricsServiceIntegration();


// Add RABBIT_MQ listener / subscriber:
// ... register MessageBusSubscriber as Singleton for DI access:
builder.Services.AddSingleton<MessageBusSubscriber>();
// ... deploy MessageBusSubscriber singleton service as a Background Service for AMQP listenning:
builder.Services.AddHostedService(provider => provider.GetService<MessageBusSubscriber>());

builder.Services.AddDbContext<InventoryContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddManagementServiceIntegration(builder.Configuration);

builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>().AddHttpMessageHandler<Metrics_HttpClientRequest_INTERCEPTOR>().AddHttpMessageHandler<Management_HttpClientRequest_INTERCEPTOR>();

builder.Services.AddSingleton<IItemRepository, ItemRepository>();
builder.Services.AddSingleton<IItemService, ItemService>();
builder.Services.AddSingleton<IServiceResultFactory, ServiceResultFactory>();

builder.Services.AddTransient<ConsoleWriter>();

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


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();


app.UseMiddleware<Metrics_MW>();

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

PrepDB.PrepPopulation(app, app.Environment.IsProduction(), app.Configuration);

// true/false - load the config from Management service at startup:
GlobalConfig_Seed.Load(app, true);

app.Run();
