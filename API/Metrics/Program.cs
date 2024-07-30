using Business.Data;
using Business.Data.Tools.Interfaces;
using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Clients.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Tools;
using Business.Metrics.Http.Clients;
using Business.Metrics.Http.Clients.Interfaces;
using Business.Metrics.Http.Services;
using Business.Metrics.Http.Services.Interfaces;
using Business.Middlewares;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

Management_Register.Register(builder);
builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();
builder.Services.AddSingleton<IExId, ExId>();
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<IHttpClient_Metrics, HttpClient_Metrics>(); 
builder.Services.AddScoped<IHttpAppClient, HttpAppClient>();


var app = builder.Build();


app.UseMiddleware<ErrorHandler_MW>();

app.UseMiddleware<Metrics_MW>();

GlobalConfig_Seed.Load(app);

app.MapControllers();

app.Run();
