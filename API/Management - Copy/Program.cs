using Business.Middlewares;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<Metrics_MW>();

app.UseMiddleware<ServiceId_MW>();

// Custom Exception Handler:
app.UseMiddleware<ErrorHandler_MW>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.MapGet("/", () => "Zedous !");

app.MapControllers();

app.Run();
