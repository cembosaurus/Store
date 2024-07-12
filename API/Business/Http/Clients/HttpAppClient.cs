using Business.Http.Clients.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Business.Http.Clients
{
    public class HttpAppClient : IHttpAppClient
    {

        private readonly HttpClient _httpClient;
        private HttpResponseMessage _result;
        private IHttpContextAccessor _accessor;
        private readonly string _serviceName;
        private string _remoteServiceName;
        private DateTime _timeIn;


        public HttpAppClient(HttpClient httpClient, IHttpContextAccessor accessor, IConfiguration config)
        {
            _httpClient = httpClient;
            _accessor = accessor;
            _serviceName = config.GetSection("Name").Value;
            _serviceName = !string.IsNullOrWhiteSpace(_serviceName) ? _serviceName : Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
        }





        public async Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage, string remoteServiceName)
        {
            _timeIn = DateTime.UtcNow;
            _remoteServiceName = remoteServiceName;

            _accessor.HttpContext?.Response.Headers.Append($"Metrics.{_serviceName}", $"CLIENT.{_serviceName}.TO.{_remoteServiceName}.AT.{_timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");

            _result = await _httpClient.SendAsync(requestMessage);

            var response = _result.Headers.Where(h => h.Key.StartsWith($"Metrics."));
            foreach (var header in response)
            { 
                _accessor.HttpContext?.Response.Headers.Append(header.Key, header.Value.ToArray());
            }

            _accessor.HttpContext?.Response.Headers.Append($"Metrics.{_serviceName}", $"CLIENT.{_serviceName}.FROM.{_remoteServiceName}.AT.{_timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");

            return _result;
        }

    }
}
