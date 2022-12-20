using API_Gateway.HttpServices.Identity;
using API_Gateway.HttpServices.Identity.Interfaces;
using API_Gateway.HttpServices.Inventory;
using API_Gateway.HttpServices.Inventory.Interfaces;
using API_Gateway.HttpServices.Ordering;
using API_Gateway.HttpServices.Ordering.Interfaces;
using API_Gateway.Services.Identity;
using API_Gateway.Services.Identity.Interfaces;
using API_Gateway.Services.Inventory;
using API_Gateway.Services.Inventory.Interfaces;
using API_Gateway.Services.Ordering;
using API_Gateway.Services.Ordering.Interfaces;
using Business.Identity.Enums;
using Business.Identity.Http.Clients;
using Business.Identity.Http.Clients.Interfaces;
using Business.Inventory.Http;
using Business.Inventory.Http.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Middlewares;
using Business.Ordering.Http;
using Business.Ordering.Http.Interfaces;
using Business.Ordering.Http.temp;
using Business.Scheduler.JWT;
using Business.Scheduler.JWT.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IJWTTokenStore, JWTTokenStore>();

builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<ICatalogueItemService, CatalogueItemService>();
builder.Services.AddTransient<IItemPriceService, ItemPriceService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<ICartItemService, CartItemService>();
builder.Services.AddTransient<IOrderService, OrderService>();

builder.Services.AddTransient<IHttpIdentityService, HttpIdentityService>();
builder.Services.AddTransient<IHttpUserService, HttpUserService>();
builder.Services.AddTransient<IHttpAddressService, HttpAddressService>();
builder.Services.AddTransient<IHttpItemService, HttpItemService>();
builder.Services.AddTransient<IHttpCatalogueItemService, HttpCatalogueItemService>();
builder.Services.AddTransient<IHttpItemPriceService, HttpItemPriceService>();
builder.Services.AddTransient<IHttpCartService, HttpCartService>();
builder.Services.AddTransient<IHttpCartItemService, HttpCartItemService>();
builder.Services.AddTransient<IHttpOrderService, HttpOrderService>();

builder.Services.AddHttpClient<IHttpIdentityClient, HttpIdentityClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetSection("RemoteServices:IdentityService").Value);
});
builder.Services.AddHttpClient<IHttpAddressClient, HttpAddressClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetSection("RemoteServices:IdentityService").Value);
});
builder.Services.AddHttpClient<IHttpUserClient, HttpUserClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetSection("RemoteServices:IdentityService").Value);
});
builder.Services.AddHttpClient<IHttpItemClient, HttpItemClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetSection("RemoteServices:InventoryService").Value);
});
builder.Services.AddHttpClient<IHttpCatalogueItemClient, HttpCatalogueItemClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetSection("RemoteServices:InventoryService").Value);
});
builder.Services.AddHttpClient<IHttpItemPriceClient, HttpItemPriceClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetSection("RemoteServices:InventoryService").Value);
});
builder.Services.AddHttpClient<IHttpCartClient, HttpCartClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetSection("RemoteServices:OrderingService").Value);
});
builder.Services.AddHttpClient<IHttpCartItemClient, HttpCartItemClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetSection("RemoteServices:OrderingService").Value);
});
builder.Services.AddHttpClient<IHttpOrderClient, HttpOrderClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetSection("RemoteServices:OrderingService").Value);
});



builder.Services.AddTransient<IServiceResultFactory, ServiceResultFactory>();

// Middleware that authenticate request before hitting controller (endpoint):
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    var secret = builder.Configuration.GetSection("AppSettings:Key").Value;
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




//var devCorsPolicy = "devCorsPolicy";
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(devCorsPolicy, builder => {
//        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//    });
//});



builder.Services.AddSwaggerGen();

var app = builder.Build();

// Custom Exception Handler:
app.UseMiddleware<ErrorHandlerMiddleware>();


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

app.Run();
