using Business.Metrics.DTOs;
using Business.Metrics.Http.Services.Interfaces;
using Business.Metrics.Services;
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
        // AppId - identifies instance (K8 replica) of service:
        private static readonly Guid _appId = Guid.NewGuid();
        private static readonly DateTime _deployed = DateTime.UtcNow;
        private static readonly AppId_MODEL _appId_Model = new() { AppId = _appId, Deployed = _deployed };

        private readonly RequestDelegate _next;
        private IMetricsData _metricsData;
        private ConsoleWriter _cw;
        private readonly string _thisService;
        private StringValues _requestFrom;
        private int _index;
        private IHttpMetricsService _httpMetricsService;



        public Metrics_MW(RequestDelegate next, IConfiguration config, IMetricsData metricsData)
        {
            _metricsData = metricsData;
            _thisService = config.GetSection("Metrics:Name").Value ?? Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
            _next = next;
        }





        public async Task Invoke(HttpContext context, IHttpMetricsService httpMetricsService, ConsoleWriter cw)
        {
            _cw = cw;
            _httpMetricsService = httpMetricsService;

            await AppId(context);

            if (_thisService != "MetricsService")
                await RequestHandler(context);

            await _next(context);
        }




        private async Task RequestHandler(HttpContext context)
        {
            _metricsData.Initialize();

            Request_IN(context);

            context.Response.OnStarting(async () =>
            {
                Response_OUT(context);

                if (_requestFrom == "client")
                    await ReportMetrics(context);

                return;
            });
        }




        private void Request_IN(HttpContext context)
        {
            // metrics START:

            // index in incoming request header, if null set to 1 else increase it by 1:
            _index = context.Request.Headers.TryGetValue("Metrics.Index", out StringValues indexStrArr) ? (int.TryParse(indexStrArr[0], out int indexInt) ? ++indexInt : 1) : 1;
            _requestFrom = context.Request.Headers.TryGetValue("Metrics.RequestFrom", out _requestFrom) ? _requestFrom[0] : "client";

            // pass index into http client:
            _metricsData.Index = _index;

            _metricsData.AddHeader($"Metrics.{_thisService}.{_appId}", $"{_index}.REQ.IN.{_requestFrom}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}.{RequestURL(context)}");
        }




        private void Response_OUT(HttpContext context)
        {
            // metrics END:

            // increment and read index passed from http client:
            _index = ++_metricsData.Index;

            // passing this app name and index back into caller app.
            // client doesn't need it:
            if (_requestFrom != "client")
            { 
                context.Response.Headers.Remove("Metrics.Index");
                context.Response.Headers.TryAdd("Metrics.Index", _index.ToString());

                context.Response.Headers.Remove("Metrics.ResponseFrom");
                context.Response.Headers.TryAdd("Metrics.ResponseFrom", _thisService);            
            }

            _metricsData.AddHeader($"Metrics.{_thisService}.{_appId}", $"{_index}.RESP.OUT.{_requestFrom}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");

            // append headers from incoming response in http client further down to caller app:
            foreach (var h in _metricsData.Headers)
            { 
                context.Response.Headers.Append(h.Key, h.Value);
            }
        }



        private async Task ReportMetrics(HttpContext context)
        {
            // send data collected from whole chain of HTTP requests to Metrics API serevice:
        
            context.Response.Headers.Remove("Metrics.Index");
        
            var metricsData = context.Response.Headers
                .Where(rh => rh.Key.StartsWith("Metrics.") && rh.Value.Any())
                .Select(s => new KeyValuePair<string, string[]>(s.Key, s.Value!))
                .ToList();
        
            _cw.Message("HTTP Post (outgoing): ", _httpMetricsService.GetRemoteServiceName, $"{context.Request.Host}{context.Request.Path}", Enums.TypeOfInfo.INFO, $"Measured request: '{_thisService}' {context.Request.Host}{context.Request.Path}");
        
            var metricsHttpResult = await _httpMetricsService.Update(new MetricsCreateDTO { Data = metricsData });
        
            _cw.Message("HTTP Response (incoming): ", _httpMetricsService.GetRemoteServiceName, $"{context.Request.Host}{context.Request.Path}", metricsHttpResult.Status ? Enums.TypeOfInfo.SUCCESS : Enums.TypeOfInfo.FAIL, metricsHttpResult != null ? metricsHttpResult.Message : "Response not received !");
        }




        private async Task AppId(HttpContext context)
        {
            if (context.Request.Path == "/appid")
                context.Response.Headers.Append($"AppId", $"{_thisService}.{_appId}");

        }


        private static string RequestURL(HttpContext context)
        {
            return $"{context.Request.Method ?? ""}.{context.Request.Host.Host ?? ""}.{context.Request.Host.Port}.{context.Request.Path.Value ?? ""}";
        }



        public static AppId_MODEL AppId_Model
        {
            get { return _appId_Model; }
        }



        public class AppId_MODEL
        {
            public Guid AppId = Guid.Empty;
            public string AppName = "";
            public string ServiceName = "";
            public DateTime Deployed;

        }

    }

}
