using Business.Metrics.Http.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly ConsoleWriter _cw;
        private readonly string _thisService;
        private StringValues _requestFrom;
        private bool _isMetricsReporter;
        private int _index;
        private IHttpMetricsService? _httpMetricsService;



        public Metrics_MW(RequestDelegate next, IConfiguration config, ConsoleWriter cw)
        {
            _cw = cw;
            _thisService = config.GetSection("Metrics:Name").Value ?? Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
            _next = next;
        }





        public async Task Invoke(HttpContext context, IHttpMetricsService httpMetricsService)
        {
          _httpMetricsService = httpMetricsService;

            await AppId(context);

            if (_thisService != "MetricsService")
                await RequestHandler(context);

            await _next(context);
        }




        private async Task RequestHandler(HttpContext context)
        {
            Request_IN(context);

            context.Response.OnStarting(async () =>
            {
                _isMetricsReporter = _index == 1;

                Response_OUT(context);
            
                await ReportMetrics(context);

                return;
            });
        }




        private void Request_IN(HttpContext context)
        {
            // metrics START:

            _index = context.Request.Headers.TryGetValue("Metrics.Index", out StringValues indexStrArr) ? (int.TryParse(indexStrArr[0], out int indexInt) ? ++indexInt : 1) : 1;
            _requestFrom = context.Request.Headers.TryGetValue("Metrics.RequestFrom", out _requestFrom) ? _requestFrom[0] : "client";

            // pass index into http client via context:
            context.Request.Headers.Remove("Metrics.Index");
            context.Request.Headers.Add("Metrics.Index", _index.ToString());

            // add METRICS header into this app response.
            // It will be passed back to calling API service on the way back:
            context.Response.Headers.Append($"Metrics.{_thisService}.{_appId}", $"{_index}.REQ.IN.{_requestFrom}.{RequestURL(context)}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
        }




        private void Response_OUT(HttpContext context)
        {
            // metrics END:

            _index = context.Response.Headers.TryGetValue("Metrics.Index", out StringValues indexStrArr) 
                ? (int.TryParse(indexStrArr[0], out int indexInt) ? ++indexInt : ++_index) 
                : ++_index;

            // pass the values back into caller app:
            context.Response.Headers.Remove("Metrics.Index");
            if(_requestFrom != "client")
                context.Response.Headers.Add("Metrics.Index", _index.ToString());

            // add METRICS header into this app response.
            // It will be passed back to calling API service in http response:
            context.Response.Headers.Append($"Metrics.{_thisService}.{_appId}", $"{_index}.RESP.OUT.{_requestFrom}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
        }



        private async Task ReportMetrics(HttpContext context)
        {

            // send data collected from whole chain of HTTP requests to Metrics API serevice:
            if (_isMetricsReporter)
            {
                context.Response.Headers.Remove("Metrics.Index");

                var metricsData = context.Response.Headers.Where(rh => rh.Key.StartsWith("Metrics.")).Select(s => new KeyValuePair<string, string[]>(s.Key, s.Value.ToArray())).ToList();

                _cw.Message("HTTP Post (outgoing): ", _httpMetricsService.GetRemoteServiceName, $"{context.Request.Host}{context.Request.Path}", Enums.TypeOfInfo.INFO, $"Measured request: '{_thisService}' {context.Request.Host}{context.Request.Path}");

                var metricsHttpResult = await _httpMetricsService.Update(metricsData);
                
                _cw.Message("HTTP Response (incoming): ", _httpMetricsService.GetRemoteServiceName, $"{context.Request.Host}{context.Request.Path}", metricsHttpResult.Status ? Enums.TypeOfInfo.SUCCESS : Enums.TypeOfInfo.FAIL, metricsHttpResult != null ? metricsHttpResult.Message : "Response not received !");

            }

            _isMetricsReporter = default;
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
