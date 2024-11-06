using Business.Data;
using Business.Data.Tools.Interfaces;
using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Clients.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Data;
using Business.Management.DI;
using Business.Metrics.Http.Clients;
using Business.Metrics.Http.Clients.Interfaces;
using Business.Metrics.Http.Services;
using Business.Metrics.Http.Services.Interfaces;
using Business.Middlewares;
using Business.Tools;
using Metrics.Data;
using Metrics.Data.Repositories;
using Metrics.Data.Repositories.Interfaces;
using Metrics.Services;
using Metrics.Services.Interfaces;
using Metrics.Services.ServicesTools;
using Metrics.Services.ServicesTools.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

ManagementService_DI.Register(builder);

builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();
builder.Services.AddSingleton<IExId, ExId>();
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<IHttpClient_Metrics, HttpClient_Metrics>(); 
builder.Services.AddScoped<IHttpAppClient, HttpAppClient>();

builder.Services.AddDbContext<MetricsContext>();

builder.Services.AddScoped<ICollectorService, CollectorService>();
builder.Services.AddScoped<IMetricsDataProcessor, MetricsDataProcessor>();
builder.Services.AddScoped<IMetricsRepository, MetricsRepository>();

builder.Services.AddTransient<ConsoleWriter>();


var app = builder.Build();


app.UseMiddleware<ErrorHandler_MW>();

app.UseMiddleware<Metrics_MW>();

GlobalConfig_Seed.Load(app);

app.MapControllers();

app.Run();
