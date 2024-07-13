using Business.Data.Tools.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;



namespace Business.Middlewares
{
    public class Metrics_Client_MW
    {
        private RequestDelegate _next;
        private readonly Guid _appId;
        private readonly string _serviceName;


        public Metrics_Client_MW(RequestDelegate next, IConfiguration config, IGlobalVariables gv)
        {
            _appId = gv.AppID;
            _serviceName = config.GetSection("Name").Value ?? Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var _timeIn = DateTime.UtcNow;

            context.Response.Headers.Append($"Metrics.{_serviceName}.{_appId}", $"MIDDLEWARE.IN.{""}.{_timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");

            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append($"Metrics.{_serviceName}.{_appId}", $"MIDDLEWARE.OUT.{""}.{_timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");

                return Task.CompletedTask;
            });

            await _next(context);

        }
    }
}
