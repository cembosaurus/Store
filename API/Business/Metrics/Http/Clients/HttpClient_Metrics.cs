using Business.Metrics.Http.Clients.Interfaces;
using Business.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Globalization;



namespace Business.Metrics.Http.Clients
{
    public class HttpClient_Metrics : IHttpClient_Metrics
    {

        protected readonly HttpClient _httpClient;
        private HttpRequestMessage? _requestMessage;
        private HttpResponseMessage? _responseMessage;
        private IHttpContextAccessor _accessor;
        private readonly string _thisService;
        private string? _sendToService;
        private bool _metricsDataSender;
        private readonly Guid _appId;
        private int _index;



        public HttpClient_Metrics(HttpClient httpClient, IHttpContextAccessor accessor, IConfiguration config)
        {
            _httpClient = httpClient;
            _accessor = accessor;
            _appId = Metrics_MW.AppId_Model.AppId;
            _thisService = config.GetSection("Metrics:Name").Value;
            _thisService = !string.IsNullOrWhiteSpace(_thisService) ? _thisService : Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
        }





        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            _requestMessage = requestMessage;

            _index = _accessor.HttpContext?.Request.Headers.TryGetValue("Metrics.Index", out StringValues indexStrArr) ?? false ? (int.TryParse(indexStrArr[0], out int indexInt) ? indexInt : 0) : 0;
            _metricsDataSender = _accessor.HttpContext?.Request.Headers.Any(rh => rh.Key == "Metrics.Reporter") ?? false;
            _sendToService = _requestMessage.Options.TryGetValue(new HttpRequestOptionsKey<string>("RequestTo"), out _sendToService) ? _sendToService : "not_specified";



            MetricsEnd();


            try
            {
                _responseMessage = await _httpClient.SendAsync(requestMessage);     // To Do: sometimes on startup when Metrics API service is not ON yet, it gives NULL ref EX !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("HTTP Client: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"response from {_sendToService}: {ex.Message}");
                Console.ResetColor();

                throw;
            }
            finally 
            {
                _responseMessage = _responseMessage ?? new HttpResponseMessage();

                _responseMessage?.Headers.Add("Metrics.Index", (_index++).ToString());

                MetricsStart(); 
            }


            MetricsStart();


            return _responseMessage;
        }



        public HttpClient HtpClient => _httpClient;




        private void MetricsEnd()
        {
            // request out:

            _requestMessage?.Headers.Add("Metrics.Index", (++_index).ToString());

            if (_metricsDataSender)
                _requestMessage?.Headers.Add("Metrics.Reporter", _metricsDataSender.ToString());

            _requestMessage?.Headers.Add("Metrics.RequestFrom", _thisService);

            _accessor.HttpContext?.Response.Headers.Append(
                $"Metrics.{_thisService}.{_appId}",
                $"{_index}.REQ.OUT.{_sendToService}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
                );
        }

        private void MetricsStart()
        {
            // response in:

            _index = _responseMessage.Headers.TryGetValues("Metrics.Index", out IEnumerable<string>? indexStrArr) ? (int.TryParse(indexStrArr?.ElementAt(0), out int indexInt) ? ++indexInt : 0) : 0;

            var responseMetrics = _responseMessage.Headers.Where(h => h.Key.StartsWith($"Metrics."));
            var responseAppID = _responseMessage.Headers.Where(h => h.Key.StartsWith($"AppId."));

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

            _accessor.HttpContext?.Response.Headers.Remove("Metrics.Index");
            _accessor.HttpContext?.Response.Headers.Add("Metrics.Index", _index.ToString());

            _accessor.HttpContext?.Response.Headers.Append(
                $"Metrics.{_thisService}.{_appId}",
                $"{_index}.RESP.IN.{_sendToService}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
                );
        }

    }
}
