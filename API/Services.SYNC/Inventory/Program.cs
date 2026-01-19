using Business.Data;
using Business.Data.DB.DI;
using Business.Data.Tools.Interfaces;
using Business.Exceptions;
using Business.Exceptions.Interfaces;
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
using FluentValidation;
using FluentValidation.AspNetCore;
using Inventory.Middlewares;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Inventory.Data;
using Services.Inventory.Data.Repositories.Interfaces;
using System.Text;




var builder = WebApplication.CreateBuilder(args);

DBContext_DI.Register<InventoryContext>(builder);

builder.Services.AddIdentityServiceIntegration(builder.Configuration);
builder.Services.AddManagementServiceIntegration(builder.Configuration);
builder.Services.AddMetricsServiceIntegration();

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

// true/false - load the config from Management service at startup:
GlobalConfig_Seed.Load(app, true);

app.Run();
