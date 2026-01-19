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
using Business.Payment.Http.Services;
using Business.Payment.Http.Services.Interfaces;
using Business.Scheduler.Http.Services;
using Business.Scheduler.Http.Services.Interfaces;
using Business.Tools;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ordering.Data;
using Ordering.Data.Repositories;
using Ordering.Data.Repositories.Interfaces;
using Ordering.Middlewares;
using Ordering.Services;
using Ordering.Services.Interfaces;
using Ordering.Tools;
using Ordering.Tools.Interfaces;
using System.Reflection;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

DBContext_DI.Register<OrderingContext>(builder);






// TEST: ----------------------------------
builder.Services.AddHttpContextAccessor();








// add validation filter in front of controller:
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidationFilter>();
});

builder.Services.AddIdentityServiceIntegration(builder.Configuration);
builder.Services.AddManagementServiceIntegration(builder.Configuration);
builder.Services.AddMetricsServiceIntegration();

builder.Services.AddSingleton<IExId, ExId>(); 
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();
builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();

builder.Services.AddFluentValidation(conf => {
    conf.DisableDataAnnotationsValidation = true;
    //conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);                    // scans for validations in this poroject
    conf.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());     // scans for validations in all linked projects or libraries
    conf.AutomaticValidationEnabled = true;
});

builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });         // Allow optional argument in controller's action

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IArchiveService, ArchiveService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IHttpItemService, HttpItemService>();
builder.Services.AddScoped<IHttpCatalogueItemService, HttpCatalogueItemService>();
builder.Services.AddScoped<IHttpItemPriceService, HttpItemPriceService>();
builder.Services.AddScoped<IHttpAddressService, HttpAddressService>();
builder.Services.AddScoped<IHttpPaymentService, HttpPaymentService>();
builder.Services.AddScoped<IHttpSchedulerService, HttpSchedulerService>();

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>().AddHttpMessageHandler<Metrics_HttpClientRequest_INTERCEPTOR>().AddHttpMessageHandler<Management_HttpClientRequest_INTERCEPTOR>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemsRepository, CartItemsRepository>();
builder.Services.AddScoped<IArchiveRepository , ArchiveRepository>();

builder.Services.AddScoped<ICart, OrderingTools>();
builder.Services.AddScoped<IOrder, OrderingTools>();
builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<ConsoleWriter>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



app.UseMiddleware<RequestHandler_MW>();




app.UseMiddleware<Metrics_MW>();

app.UseMiddleware<ErrorHandler_MW>();

app.UseMiddleware<Ordering_DbGuard_MW>();


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

Ordering_DbGuard_MW.Migrate_DB(app);

// true/false - load the config from Management service at startup:
GlobalConfig_Seed.Load(app, true);

app.Run();
