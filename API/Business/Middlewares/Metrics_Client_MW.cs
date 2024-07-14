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
        private bool _isHttpSender;


        public Metrics_Client_MW(RequestDelegate next, IConfiguration config, IGlobalVariables gv)
        {
            _appId = gv.AppID;
            _serviceName = config.GetSection("Name").Value ?? Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var _timeIn = DateTime.UtcNow;

            var isHttpSender = context.Request.Headers.FirstOrDefault(x => x.Key == "Metrics.Sender").Value;

            _isHttpSender = !isHttpSender.Any(x => x.Equals(false.ToString()));

            context.Response.Headers.Append($"Metrics.{_serviceName}.{_appId}", $"REQ.IN.{""}.{_timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");


            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append($"Metrics.{_serviceName}.{_appId}", $"RESP.OUT.{""}.{_timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");




                // To Do: send METRICS data to metrics service if _isHttpSender !!!!
                if (_isHttpSender)
                {
                    context.Response.Headers.Append($"Metrics.Sender", $"{_serviceName}.{_appId}");
                }
                


                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
