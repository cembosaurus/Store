using Business.Data;
using Business.Data.DB.DI;
using Business.Data.Tools.Interfaces;
using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Clients.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Data;
using Business.Management.DI;
using Business.Management.Services;
using Business.Metrics.DI;
using Business.Metrics.Http.Services;
using Business.Metrics.Http.Services.Interfaces;
using Business.Metrics.Tools;
using Business.Metrics.Tools.Interfaces;
using Business.Middlewares;
using Business.Tools;
using Metrics.Data;
using Metrics.Data.Repositories;
using Metrics.Data.Repositories.Interfaces;
using Metrics.Services;
using Metrics.Services.Interfaces;
using Metrics.Services.Tools;
using Metrics.Services.Tools.Interfaces;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

DBContext_DI.Register<MetricsContext>(builder);
builder.Services.AddManagementServiceIntegration(builder.Configuration);
builder.Services.AddMetricsServiceIntegration();

builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();
builder.Services.AddSingleton<IExId, ExId>();
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>().AddHttpMessageHandler<Metrics_HttpClientRequest_INTERCEPTOR>().AddHttpMessageHandler<Management_HttpClientRequest_INTERCEPTOR>();

builder.Services.AddScoped<IMetricsDataHandler, MetricsDataHandler>();
builder.Services.AddScoped<ICollectorService, CollectorService>();
builder.Services.AddScoped<IMetricsDataTool, MetricsDataTool>();
builder.Services.AddScoped<IMetricsRepository, MetricsRepository>();

builder.Services.AddTransient<ConsoleWriter>();


var app = builder.Build();


app.UseMiddleware<Metrics_MW>();

app.UseMiddleware<ErrorHandler_MW>();

// true/false - load the config from Management service at startup:
GlobalConfig_Seed.Load(app, true);

app.MapControllers();

app.Run();
