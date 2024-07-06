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
using Business.Ordering.Http.Services;
using Business.Ordering.Http.Services.Interfaces;
using Business.Scheduler.JWT;
using Business.Scheduler.JWT.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IExId, ExId>(); 
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();

builder.Services.Configure<Config_Global_AS_MODEL>(builder.Configuration.GetSection("Config.Global"));
builder.Services.AddSingleton<Config_Global_DB>();
builder.Services.AddScoped<IConfig_Global_REPO, Config_Global_REPO>();
builder.Services.AddScoped<IGlobalConfig_PROVIDER, GlobalConfig_PROVIDER>();
builder.Services.AddScoped<IHttpManagementService, HttpManagementService>();
builder.Services.AddTransient<IAppsettings_PROVIDER, Appsettings_PROVIDER>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
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

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>();

builder.Services.AddTransient<IServiceResultFactory, ServiceResultFactory>();

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




//var devCorsPolicy = "devCorsPolicy";
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(devCorsPolicy, builder => {
//        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//    });
//});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:4001", "http://localhost:4000", "http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});



builder.Services.AddSwaggerGen();



var app = builder.Build();


app.UseMiddleware<Metrics_MW>();

app.UseMiddleware<ServiceId_MW>();

app.UseMiddleware<ErrorHandler_MW>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors(devCorsPolicy);
app.UseCors(opt => { 
    opt.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader(); 
});

app.UseAuthentication();

app.UseAuthorization();

//app.MapGet("/", () => "Zedous !");

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var remoteServicesInfoService = scope.ServiceProvider.GetRequiredService<IGlobalConfig_PROVIDER>();

    // load all remote services models from, Management service:
    try { 
        var result = await remoteServicesInfoService.ReLoadRemoteServices();
        Console.WriteLine($"--> Loading remote services info models from Management service: {(result.Status ? "Success !" : $"Failed: {result.Message}")}");
    }
    catch (HttpRequestException ex) {
        Console.WriteLine($"--> Remote service info models were NOT loaded from Management service ! EX: {ex.Message}");
    }
}


app.Run();
