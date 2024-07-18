using Business.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Globalization;



namespace Business.Metrics
{
    public class HttpMetricsClient
    {

        protected readonly HttpClient _httpClient;
        private HttpRequestMessage _requestMessage;
        private HttpResponseMessage _result;
        private IHttpContextAccessor _accessor;
        private readonly string _thisService;
        private string? _sendToService;
        private bool _metricsDataSender;
        private readonly Guid _appId;



        public HttpMetricsClient(HttpClient httpClient, IHttpContextAccessor accessor, IConfiguration config)
        {
            _httpClient = httpClient;
            _accessor = accessor;
            _appId = Metrics_MW.AppId_Model.AppId;
            _thisService = config.GetSection("Name").Value;
            _thisService = !string.IsNullOrWhiteSpace(_thisService) ? _thisService : Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
        }





        protected virtual async Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage)
        {
            _requestMessage = requestMessage;

            _sendToService = _requestMessage.Options.TryGetValue(new HttpRequestOptionsKey<string>("RequestTo"), out _sendToService) ? _sendToService : "not_specified";
            _metricsDataSender = _accessor.HttpContext?.Request.Headers.Any(rh => rh.Key == "Metrics.DataSender") ?? false;



            MetricsEnd();


            _result = await _httpClient.SendAsync(requestMessage);


            MetricsStart();

            return _result;
        }


        private void MetricsEnd()
        {
            // request out:

            if(_metricsDataSender)
                _requestMessage.Headers.Add("Metrics.DataSender", _metricsDataSender.ToString());

            _requestMessage.Headers.Add("Metrics.RequestFrom", _thisService);

            _accessor.HttpContext?.Response.Headers.Append(
                $"Metrics.{_thisService}.{_appId}",
                $"REQ.OUT.{_sendToService}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
                );
        }

        private void MetricsStart()
        {
            // response in:

            var responseMetrics = _result.Headers.Where(h => h.Key.StartsWith($"Metrics."));
            var responseAppID = _result.Headers.Where(h => h.Key.StartsWith($"AppId."));

            if (responseMetrics != null)
            {
                foreach (var header in responseMetrics)
                {
                    _accessor.HttpContext?.Response.Headers.Append(header.Key, header.Value.ToArray());
                }
            }

            if (responseAppID != null)
            {
                foreach (var header in responseAppID)
                {
                    _accessor.HttpContext?.Response.Headers.Append(header.Key, header.Value.ToArray());
                }
            }

            _accessor.HttpContext?.Response.Headers.Append(
                $"Metrics.{_thisService}.{_appId}",
                $"RESP.IN.{_sendToService}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
                );
        }

    }
}
