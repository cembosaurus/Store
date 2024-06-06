using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Business.Middlewares
{
    public class Metrics_MW
    {
        private RequestDelegate _next;
        private DateTime _timeIn;

        public Metrics_MW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            _timeIn = DateTime.UtcNow;

           
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("TimeIn", _timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
                context.Response.Headers.Add("TimeOut", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));

                return Task.CompletedTask;
            });

            await _next(context);

        }
    }
}
