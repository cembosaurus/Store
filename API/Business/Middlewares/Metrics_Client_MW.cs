using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;



namespace Business.Middlewares
{
    public class Metrics_Client_MW
    {

        private RequestDelegate _next;
        private readonly string _serviceName;


        public Metrics_Client_MW(RequestDelegate next, IConfiguration config)
        {
            _serviceName = config.GetSection("Name").Value;
            _serviceName = !string.IsNullOrWhiteSpace(_serviceName) ? _serviceName : Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var _timeIn = DateTime.UtcNow;

            context.Response.Headers.Append($"Metrics.{_serviceName}", $"MW.{_serviceName}.IN.{""}.AT.{_timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");

            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append($"Metrics.{_serviceName}", $"MW.{_serviceName}.OUT.{""}.AT.{_timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");

                return Task.CompletedTask;
            });

            await _next(context);

        }
    }
}
