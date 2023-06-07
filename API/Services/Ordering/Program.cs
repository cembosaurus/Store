using Business.Identity.Enums;
using Business.Identity.Http.Clients.Interfaces;
using Business.Inventory.Http;
using Business.Inventory.Http.Interfaces;
using Business.Libraries.Http.Interfaces;
using Business.Libraries.Http;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Payment.Http;
using Business.Payment.Http.Interfaces;
using Business.Scheduler.Http;
using Business.Scheduler.Http.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ordering.Data;
using Ordering.Data.Repositories;
using Ordering.Data.Repositories.Interfaces;
using Ordering.HttpServices;
using Ordering.HttpServices.Interfaces;
using Ordering.OrderingBusinessLogic;
using Ordering.OrderingBusinessLogic.Interfaces;
using Ordering.Services;
using Ordering.Services.Interfaces;
using System.Text;
using Business.Identity.Http.Clients;
using MediatR;
using System.Reflection;
using Business.Filters.Validation;
using FluentValidation.AspNetCore;
using Business.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidationFilter>();
});

builder.Services.AddFluentValidation(conf => {
    conf.DisableDataAnnotationsValidation = true;
    //conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);                    // scans for validations in this poroject
    conf.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());     // scans for validations in all linked projects or libraries
    conf.AutomaticValidationEnabled = true;
});

builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });         // Allow optional argument in controller's action

builder.Services.AddDbContext<OrderingContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("OrderingConnStr")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IArchiveService, ArchiveService>();
builder.Services.AddScoped<IHttpInventoryService, HttpInventoryService>();
builder.Services.AddScoped<IHttpAddressService, HttpAddressService>();
builder.Services.AddScoped<IHttpPaymentService, HttpPaymentService>();
builder.Services.AddScoped<IHttpSchedulerService, HttpSchedulerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddHttpClient<IHttpItemPriceClient, HttpItemPriceClient>();
builder.Services.AddHttpClient<IHttpItemClient, HttpItemClient>();
builder.Services.AddHttpClient<IHttpCatalogueItemClient, HttpCatalogueItemClient>();
builder.Services.AddHttpClient<IHttpIdentityClient, HttpIdentityClient>();
builder.Services.AddHttpClient<IHttpPaymentClient, HttpPaymentClient>();
builder.Services.AddHttpClient<IHttpSchedulerClient, HttpSchedulerClient>();
builder.Services.AddHttpClient<IHttpAddressClient, HttpAddressClient>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemsRepository, CartItemsRepository>();
builder.Services.AddScoped<IArchiveRepository , ArchiveRepository>();

builder.Services.AddScoped<ICartBusinessLogic, OrderingTools>();
builder.Services.AddScoped<IOrderBusinessLogic, OrderingTools>();    //............. not sure if hat's necessary, maybe one instance can be used with all interfaces in code ....
builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Middleware that authenticate request before hitting controller (endpoint):
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    var secret = builder.Configuration.GetSection("AppSettings:JWTKey").Value;
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

// Custom Exception Handler:
app.UseMiddleware<ErrorHandlerMiddleware>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        Console.WriteLine("--> Ordering service: MIGRATION...");
        var db = scope.ServiceProvider.GetRequiredService<OrderingContext>();
        db.Database.Migrate();
    }
}

app.Run();
