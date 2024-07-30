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
using StaticContent.Services;
using StaticContent.Services.Interfaces;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IExId, ExId>();
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();

Management_Register.Register(builder);

builder.Services.AddScoped<IHttpMetricsService, HttpMetricsService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IImageFilesService, ImageFilesService>();

builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();

builder.Services.AddHttpClient<IHttpClient_Metrics, HttpClient_Metrics>(); 
builder.Services.AddScoped<IHttpAppClient, HttpAppClient>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:5000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.UseMiddleware<ErrorHandler_MW>();

app.UseMiddleware<Metrics_MW>();


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

app.MapControllers();

app.UseDefaultFiles();

app.UseStaticFiles();

GlobalConfig_Seed.Load(app);

app.Run();
