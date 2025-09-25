using Business.Data;
using Business.Data.Tools.Interfaces;
using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Filters.Validation;
using Business.Http.Clients;
using Business.Http.Clients.Interfaces;
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
using FluentValidation;
using FluentValidation.AspNetCore;
using Inventory.Middlewares;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Inventory.Data;
using Services.Inventory.Data.Repositories.Interfaces;
using System.Text;




var builder = WebApplication.CreateBuilder(args);




// TEST:
Console.WriteLine($"----> {Environment.GetEnvironmentVariable("DB_HOST")}");
Console.WriteLine($"----> {Environment.GetEnvironmentVariable("DB_NAME")}");
Console.WriteLine($"----> {Environment.GetEnvironmentVariable("DB_USER")}");
Console.WriteLine($"----> {Environment.GetEnvironmentVariable("DB_PASSWORD")}");
var prodConnStr = builder.Configuration["ConnectionStrings:InventoryConnStr:Prod"];
if (!string.IsNullOrEmpty(prodConnStr))
{
    prodConnStr = prodConnStr
        .Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "")
        .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "")
        .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "")
        .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "");

    //builder.Configuration["ConnectionStrings:InventoryConnStr:Prod"] = prodConnStr;
}

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

Console.WriteLine($"----> Final Connection String: {prodConnStr}");

builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlServer(prodConnStr, sql => sql.EnableRetryOnFailure()));






ManagementService_DI.Register(builder);
MetricsService_DI.Register(builder);

builder.Services.AddSingleton<IExId, ExId>();
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();

builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>().AddHttpMessageHandler<Metrics_HttpClientRequest_INTERCEPTOR>().AddHttpMessageHandler<Management_HttpClientRequest_INTERCEPTOR>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidationFilter>();
});

//builder.Services.AddFluentValidation(conf => {
//    conf.DisableDataAnnotationsValidation = true;
//    //conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);                    // scans for validations in this poroject
//    conf.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());     // scans for validations in all linked projects or libraries
//    conf.AutomaticValidationEnabled = true;
//});
builder.Services.AddFluentValidationAutoValidation(opt => opt.DisableDataAnnotationsValidation = true);
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(ValidationFilter).Assembly);

// Allow optional argument in controller's action
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });    

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ICatalogueItemService, CatalogueItemService>();
builder.Services.AddScoped<IItemPriceService, ItemPriceService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemPriceRepository, ItemPriceRepository>();
builder.Services.AddScoped<ICatalogueItemRepository, CatalogueItemRepository>();
builder.Services.AddTransient<IServiceResultFactory, ServiceResultFactory>();

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



app.UseMiddleware<Metrics_MW>();

app.UseMiddleware<ErrorHandler_MW>();

app.UseMiddleware<Inventory_DbGuard_MW>();


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

Inventory_DbGuard_MW.Seed(app);

GlobalConfig_Seed.Load(app);

app.Run();
