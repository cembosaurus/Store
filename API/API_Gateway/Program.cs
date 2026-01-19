using API_Gateway.Services.Business.Identity;
using API_Gateway.Services.Business.Identity.Interfaces;
using API_Gateway.Services.Business.Inventory;
using API_Gateway.Services.Business.Inventory.Interfaces;
using API_Gateway.Services.Business.Ordering;
using API_Gateway.Services.Business.Ordering.Interfaces;
using Business.Data;
using Business.Data.Tools.Interfaces;
using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Clients.Interfaces;
using Business.Identity.DI;
using Business.Identity.Http.Services;
using Business.Identity.Http.Services.Interfaces;
using Business.Inventory.Http.Services;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Data;
using Business.Management.DI;
using Business.Management.Services;
using Business.Metrics.DI;
using Business.Metrics.Http.Services;
using Business.Metrics.Http.Services.Interfaces;
using Business.Middlewares;
using Business.Ordering.Http.Services;
using Business.Ordering.Http.Services.Interfaces;
using Business.Scheduler.JWT;
using Business.Scheduler.JWT.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;





var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddIdentityServiceIntegration(builder.Configuration);
builder.Services.AddManagementServiceIntegration(builder.Configuration);
builder.Services.AddMetricsServiceIntegration();

builder.Services.AddSingleton<IExId, ExId>(); 
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();

builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IJWTTokenStore, JWTTokenStore>();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ICatalogueItemService, CatalogueItemService>();
builder.Services.AddScoped<IItemPriceService, ItemPriceService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IHttpIdentityService, HttpIdentityService>();
builder.Services.AddScoped<IHttpUserService, HttpUserService>();
builder.Services.AddScoped<IHttpAddressService, HttpAddressService>();
builder.Services.AddScoped<IHttpItemService, HttpItemService>();
builder.Services.AddScoped<IHttpCatalogueItemService, HttpCatalogueItemService>();
builder.Services.AddScoped<IHttpItemPriceService, HttpItemPriceService>();
builder.Services.AddScoped<IHttpCartService, HttpCartService>();
builder.Services.AddScoped<IHttpCartItemService, HttpCartItemService>();
builder.Services.AddScoped<IHttpOrderService, HttpOrderService>();

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>().AddHttpMessageHandler<Metrics_HttpClientRequest_INTERCEPTOR>().AddHttpMessageHandler<Management_HttpClientRequest_INTERCEPTOR>();

builder.Services.AddTransient<IServiceResultFactory, ServiceResultFactory>();

builder.Services.AddTransient<ConsoleWriter>();


// ---------------  Configure Serilog ---------------------------------
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", System.AppDomain.CurrentDomain.FriendlyName) // tag service
    .WriteTo.Console()
    .WriteTo.Seq("http://seq-logging-clusterip-srv:5341") // central Seq service deployed in K8. Console logg used in dev.
    .CreateLogger();

builder.Host.UseSerilog();

var devCorsPolicy = "DevCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(devCorsPolicy, policy =>
    {
        policy
            .WithOrigins(
                "http://127.0.0.1:5500",
                "http://localhost:5500",
                "http://localhost:4200",
                "http://localhost:4000",
                "https://localhost:4001"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});




builder.Services.AddSwaggerGen();



var app = builder.Build();


app.UseMiddleware<Metrics_MW>();

app.UseMiddleware<ErrorHandler_MW>();

app.UseMiddleware<Logging_MW>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(devCorsPolicy);

app.UseAuthentication();

app.UseAuthorization();

//app.MapGet("/", () => "Zedous !");

// true/false - load the config from Management service at startup:
GlobalConfig_Seed.Load(app, true);


app.MapControllers();


app.Run();
