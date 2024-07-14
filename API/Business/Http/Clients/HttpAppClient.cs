using Business.Data.Tools.Interfaces;
using Business.Http.Clients.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;



namespace Business.Http.Clients
{
    public class HttpAppClient : IHttpAppClient
    {

        private readonly HttpClient _httpClient;
        private HttpRequestMessage _requestMessage;
        private HttpResponseMessage _result;
        private IHttpContextAccessor _accessor;
        private readonly string _serviceName;
        private string? _remoteServiceName;
        private readonly Guid _appId;


        public HttpAppClient(HttpClient httpClient, IHttpContextAccessor accessor, IConfiguration config, IGlobalVariables gv)
        {
            _httpClient = httpClient;
            _accessor = accessor;
            _appId = gv.AppID;
            _serviceName = config.GetSection("Name").Value;
            _serviceName = !string.IsNullOrWhiteSpace(_serviceName) ? _serviceName : Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
        }





        public async Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage)
        {
            _requestMessage = requestMessage;

            _requestMessage.Options.TryGetValue(new HttpRequestOptionsKey<string>("RequestAPIServiceName"), out _remoteServiceName);



            MetricsEnd();
          

            _result = await _httpClient.SendAsync(requestMessage);


            MetricsStart();

            return _result;
        }



        private void MetricsEnd()
        {
            // request out:

            _requestMessage.Headers.Add("Metrics.Sender", false.ToString());

            _accessor.HttpContext?.Response.Headers.Append(
                $"Metrics.{_serviceName}.{_appId}", 
                $"REQ.OUT.{_remoteServiceName}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
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
                $"Metrics.{_serviceName}.{_appId}", 
                $"RESP.IN.{_remoteServiceName}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
                );
        }

    }
}
