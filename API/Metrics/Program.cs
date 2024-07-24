using Business.Libraries.ServiceResult.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Management.Tools;
using Business.Middlewares;
using Business.Http.Clients.Interfaces;
using Business.Http.Clients;
using Business.Exceptions.Interfaces;
using Business.Exceptions;
using Business.Data.Tools.Interfaces;
using Business.Data;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

Management_Register.Register(builder);

builder.Services.AddSingleton<IExId, ExId>();
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<IHttpAppClient, HttpAppClient>();


var app = builder.Build();


app.UseMiddleware<ErrorHandler_MW>();

app.UseMiddleware<Metrics_MW>();

GlobalConfig_Seed.Load(app);

app.Run();
