using Business.Metrics.Http.Clients.Interfaces;
using Business.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Net;



namespace Business.Metrics.Http.Clients
{
    public class HttpClient_Metrics : IHttpClient_Metrics
    {

        protected readonly HttpClient _httpClient;
        private HttpContext _context;
        private HttpRequestMessage _requestMessage;
        private HttpResponseMessage _responseMessage;
        private readonly string _thisService;
        private string? _sendToService;
        private readonly Guid _appId;
        private int _index;



        public HttpClient_Metrics(HttpClient httpClient, IHttpContextAccessor accessor, IConfiguration config)
        {
            _httpClient = httpClient;
            _context = accessor.HttpContext ?? new DefaultHttpContext();
            _appId = Metrics_MW.AppId_Model.AppId;
            _thisService = config.GetSection("Metrics:Name").Value;
            _thisService = !string.IsNullOrWhiteSpace(_thisService) ? _thisService : Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
            _requestMessage = new HttpRequestMessage();
        }





        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            _requestMessage = requestMessage;
            _sendToService = _requestMessage.Options.TryGetValue(new HttpRequestOptionsKey<string>("RequestTo"), out _sendToService) ? _sendToService : "not_specified";


            if (_sendToService != "MetricsService")
                Request_OUT();

            try
            {
                _responseMessage = await _httpClient.SendAsync(requestMessage);
            }
            catch (HttpRequestException httpReqEx)
            {
                _responseMessage = new HttpResponseMessage(httpReqEx.StatusCode ?? HttpStatusCode.ServiceUnavailable);

                throw;
            }
            finally
            {
                // Metrics service response is not measured:
                if (_sendToService != "MetricsService")
                    Response_IN(_responseMessage.StatusCode);
            }

            AppendPreviousHeadersToResponse();

            return _responseMessage;
        }



        public HttpClient HtpClient => _httpClient;



        private void Request_OUT()
        {
            // metrics END:

            // read values passed from MW in context:
            _index = _context.Request.Headers.TryGetValue("Metrics.Index", out StringValues indexStrArr) ? int.TryParse(indexStrArr[0], out int indexInt)
                // increase index for every request originated inside app (not passing through via Middelware):
                ? (indexInt <= _index ? ++_index : ++indexInt) : 0
                : 0;

            // update the values in outgoing http request.
            // message headers will be accessed by receiver as http context.Request headers:
            _requestMessage.Headers.Remove("Metrics.Index");
            _requestMessage.Headers.Add("Metrics.Index", _index.ToString());
            _requestMessage.Headers.Remove("Metrics.RequestFrom");
            _requestMessage.Headers.Add("Metrics.RequestFrom", _thisService);

            // add METRICS header into this app response.
            // It will be passed to MW after response from called API sevice, and back to caller:
            _context.Response.Headers.Append(
                $"Metrics.{_thisService}.{_appId}",
                $"{_index}.REQ.OUT.{_sendToService}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
                );
        }



        private void Response_IN(HttpStatusCode statusCode = HttpStatusCode.ServiceUnavailable)
        {
            // metrics START:

            _index = _responseMessage.Headers.TryGetValues("Metrics.Index", out IEnumerable<string>? indexStrArr) ? int.TryParse(indexStrArr?.ElementAt(0), out int indexInt)
                ? (indexInt <= _index ? ++_index : ++indexInt) : 0
                : ++_index;

            // passing increased index back into response header:
            _responseMessage.Headers.Remove("Metrics.Index");
            _responseMessage.Headers.Add("Metrics.Index", _index.ToString());

            // passing Index into MW:
            _context.Response.Headers.Remove("Metrics.Index");
            _context.Response.Headers.Add("Metrics.Index", _index.ToString());

            // add METRICS header into this app response.
            // It will be accessed in MW and send back to caller in http response:
            _context.Response.Headers.Append(
                $"Metrics.{_thisService}.{_appId}",
                $"{_index}.RESP.IN.{_sendToService}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}.{statusCode}.{(int)statusCode}"
                );
        }



        private void AppendPreviousHeadersToResponse()
        {
            // Appending Metrics headers generated in previous services in consequential main request chain to response:

            var previousServices_MetricsHeaders = _responseMessage.Headers.Where(h => h.Key.StartsWith($"Metrics."));
            var previousServices_AppIdHeaders = _responseMessage.Headers.Where(h => h.Key.StartsWith($"AppId."));

            // ADD received headers from previous requests chain:
            if (previousServices_MetricsHeaders != null)
            {
                foreach (var header in previousServices_MetricsHeaders)
                {
                    // append previous Metrics headers from requests chain:
                    _context.Response.Headers.Append(header.Key, header.Value.ToArray());
                }
            }

            if (previousServices_AppIdHeaders != null)
            {
                foreach (var header in previousServices_AppIdHeaders)
                {
                    _context.Response.Headers.Append(header.Key, header.Value.ToArray());
                }
            }
        }

    }
}