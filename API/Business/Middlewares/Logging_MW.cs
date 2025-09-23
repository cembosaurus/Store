using Microsoft.AspNetCore.Http;
using Serilog.Context;



namespace Business.Middlewares
{
    public class Logging_MW
    {
        private readonly RequestDelegate _next;

        public Logging_MW(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();

            context.Response.Headers["X-Correlation-ID"] = correlationId;

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}
