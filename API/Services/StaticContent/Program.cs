using Business.Data;
using Business.Data.Tools.Interfaces;
using Business.Exceptions;
using Business.Exceptions.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Middlewares;
using StaticContent.Services;
using StaticContent.Services.Interfaces;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IExId, ExId>(); 
builder.Services.AddSingleton<IGlobalVariables, GlobalVariables>();

builder.Services.AddScoped<IImageFilesService, ImageFilesService>();

builder.Services.AddScoped<IServiceResultFactory, ServiceResultFactory>();


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


app.UseMiddleware<Metrics_Client_MW>();

app.UseMiddleware<AppId_MW>();

app.UseMiddleware<ErrorHandler_MW>();


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

app.Run();
