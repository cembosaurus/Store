using Business.Metrics.Http.Clients.Interfaces;
using Business.Middlewares;
using Business.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Net;
using System.Web.Http;

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
            _responseMessage = default;
            _sendToService = _requestMessage.Options.TryGetValue(new HttpRequestOptionsKey<string>("RequestTo"), out _sendToService) ? _sendToService : "not_specified";


            if (_sendToService != "MetricsService")
                Request_OUT();

            try
            {
                _responseMessage = await _httpClient.SendAsync(requestMessage);
            }
            finally
            {
                if (_sendToService != "MetricsService")
                    Response_IN();
            }

            return _responseMessage;
        }



        public HttpClient HtpClient => _httpClient;




        private void Request_OUT()
        {
            // metrics END:

            _index = _accessor.HttpContext?.Request.Headers.TryGetValue("Metrics.Index", out StringValues indexStrArr) ?? false ? (int.TryParse(indexStrArr[0], out int indexInt) ? indexInt : _index) : _index;
            _accessor.HttpContext?.Request.Headers.Remove("Metrics.Index");
            _accessor.HttpContext?.Request.Headers.Add("Metrics.Index", _index.ToString());

            // increase Index and add to Request Message:
            _requestMessage?.Headers.Remove("Metrics.Index");
            _requestMessage?.Headers.Add("Metrics.Index", (++_index).ToString());

            _requestMessage?.Headers.Add("Metrics.RequestFrom", _thisService);

            _accessor.HttpContext?.Response.Headers.Append(
                $"Metrics.{_thisService}.{_appId}",
                $"{_index}.REQ.OUT.{_sendToService}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
                );
        }



        private void Response_IN()
        {
            // metrics START:

            _responseMessage = _responseMessage ?? new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);

            // increase Index and add to header:
            _index = _responseMessage.Headers.TryGetValues("Metrics.Index", out IEnumerable<string>? indexStrArr) ? (int.TryParse(indexStrArr?.ElementAt(0), out int indexInt) ? ++indexInt : _index) : ++_index;
            _responseMessage?.Headers.Remove("Metrics.Index");
            _responseMessage?.Headers.Add("Metrics.Index", (_index).ToString());
            _accessor.HttpContext?.Request.Headers.Remove("Metrics.Index");
            _accessor.HttpContext?.Request.Headers.Add("Metrics.Index", (_index).ToString());

            var responseMetrics = _responseMessage.Headers.Where(h => h.Key.StartsWith($"Metrics."));
            var responseAppID = _responseMessage.Headers.Where(h => h.Key.StartsWith($"AppId."));


            // ADD received OLD records:
            if (responseMetrics != null)
            {
                foreach (var header in responseMetrics)
                {
                    // DELETE old Index (avoid array of indexes),
                    // OR pick the last index in MiddleWare
                    // which means transporting array of Indexes instead of one Index in HTTP requests:
                    _accessor.HttpContext?.Response.Headers.Remove("Metrics.Index");
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

            // ADD this event NEW record:
            _accessor.HttpContext?.Response.Headers.Append(
                $"Metrics.{_thisService}.{_appId}",
                $"{_index}.RESP.IN.{_sendToService}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
                );
        }



    }
}
