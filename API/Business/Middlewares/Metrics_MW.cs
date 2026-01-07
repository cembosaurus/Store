using Business.Metrics.DTOs;
using Business.Metrics.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Globalization;



namespace Business.Middlewares
{
    public class Metrics_MW
    {

        // Process/pod identity (safe to keep static)
        private static readonly Guid _appId = Guid.NewGuid();
        private static readonly DateTime _deployedUtc = DateTime.UtcNow;
        private static readonly AppId_MODEL _appId_Model = new(){ AppId = _appId, DeployedUtc = _deployedUtc };
        private readonly RequestDelegate _next;
        private readonly string _thisService;

        // per-request state passed between methods
        private sealed record MetricsState(string RequestFrom);



        public Metrics_MW(RequestDelegate next, IConfiguration config)
        {
            _thisService =
                config.GetSection("Metrics:Name").Value
                ?? Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName)
                ?? "";

            _next = next;
        }





        public async Task Invoke(HttpContext context, IMetricsData metricsData, IMetricsQueue queue, ConsoleWriter cw)
        {
            AppId(context);

            if (_thisService != "MetricsService")
                RequestHandler(context, metricsData, queue, cw);

            await _next(context);
        }



        private void RequestHandler(HttpContext context, IMetricsData metricsData, IMetricsQueue queue, ConsoleWriter cw)
        {
            metricsData.Initialize();

            var state = Request_IN(context, metricsData);

            // Before response starts: add response headers for service-to-service propagation
            context.Response.OnStarting(() =>
            {
                Response_OUT(context, metricsData, state);
                return Task.CompletedTask;
            });

            // After response completes: enqueue metrics (cheap) for client-originated request
            context.Response.OnCompleted(() =>
            {
                if (state.RequestFrom == "client")
                    ReportMetrics(context, metricsData, queue, cw);

                return Task.CompletedTask;
            });
        }



        private MetricsState Request_IN(HttpContext context, IMetricsData metricsData)
        {
            var index =
                context.Request.Headers.TryGetValue("Metrics.Index", out StringValues indexStrArr)
                && int.TryParse(indexStrArr.FirstOrDefault(), out int indexInt)
                    ? indexInt + 1
                    : 1;

            var requestFrom =
                context.Request.Headers.TryGetValue("Metrics.RequestFrom", out var rf)
                    ? (rf.FirstOrDefault() ?? "client")
                    : "client";

            metricsData.Index = index;

            metricsData.AddHeader(
                $"Metrics.{_thisService}.{_appId}",
                $"{index}.REQ.IN.{requestFrom}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}.{RequestURL(context)}"
            );

            return new MetricsState(requestFrom);
        }



        private void Response_OUT(HttpContext context, IMetricsData metricsData, MetricsState state)
        {
            var index = ++metricsData.Index;

            // propagate only between services; client doesn't need these headers
            if (state.RequestFrom != "client")
            {
                context.Response.Headers["Metrics.Index"] = index.ToString();
                context.Response.Headers["Metrics.ResponseFrom"] = _thisService;
            }

            metricsData.AddHeader(
                $"Metrics.{_thisService}.{_appId}",
                $"{index}.RESP.OUT.{state.RequestFrom}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}" +
                $"{(context.Response.StatusCode == 200 ? "" : ".HTTP:" + context.Response.StatusCode)}"
            );

            // Append Metrics.* headers only for service-to-service calls
            if (state.RequestFrom != "client")
            {
                foreach (var h in metricsData.Headers) // ideally Headers returns a snapshot
                    context.Response.Headers.Append(h.Key, h.Value);
            }
        }



        private void ReportMetrics(HttpContext context, IMetricsData metricsData, IMetricsQueue queue, ConsoleWriter cw)
        {
            var payload = metricsData.Headers
                .Where(h => h.Key.StartsWith("Metrics.") && h.Value.Any())
                .Select(h => new KeyValuePair<string, string[]>(h.Key, h.Value.ToArray()))
                .ToList();

            var dto = new MetricsCreateDTO { Data = payload };

            if (!queue.TryEnqueue(dto))
            {
                cw.Message(
                    "Metrics dropped (queue full): ",
                    "MetricsService",
                    $"{context.Request.Host}{context.Request.Path}",
                    Enums.TypeOfInfo.INFO,
                    $"Measured request: '{_thisService}' {context.Request.Host}{context.Request.Path}"
                );
            }
        }



        private void AppId(HttpContext context)
        {
            if (context.Request.Path == "/appid")
                context.Response.Headers.Append("AppId", $"{_thisService}.{_appId}");
        }



        private static string RequestURL(HttpContext context)
            => $"{context.Request.Method ?? ""}.{context.Request.Host.Host ?? ""}.{context.Request.Host.Port}.{context.Request.Path.Value ?? ""}";



        public static AppId_MODEL AppId_Model => _appId_Model;



        public sealed class AppId_MODEL
        {
            public Guid AppId { get; init; }
            public DateTime DeployedUtc { get; init; }

            // don’t mutate them at runtime:
            public string AppName { get; init; } = "";
            public string ServiceName { get; init; } = "";
        }
    }
}
