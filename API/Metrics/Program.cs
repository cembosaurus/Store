using Business.Middlewares;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.UseMiddleware<Metrics_Server_MW>();

app.Run();
