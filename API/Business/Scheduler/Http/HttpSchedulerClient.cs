using Business.Scheduler.DTOs;
using Business.Scheduler.Http.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Scheduler.Http
{
    public class HttpSchedulerClient : IHttpSchedulerClient
    {

        private readonly HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpSchedulerClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:SchedulerService").Value + "/api/scheduler";
            _accessor = accessor;
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri) };
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }





        public async Task<HttpResponseMessage> CartItemsLock(CartItemsLockCreateDTO cartItemsToLock)
        {
            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_request.RequestUri + $"/cartitem/lock");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(cartItemsToLock), _encoding, _mediaType);

            Console.WriteLine($"---> LOCKING cart items .....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> CartItemsUnLock(CartItemsLockDeleteDTO cartItemsToUnLock)
        {
            _request.Method = HttpMethod.Delete;
            _request.RequestUri = new Uri(_request.RequestUri + $"/cartitem/lock");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(cartItemsToUnLock), _encoding, _mediaType);

            Console.WriteLine($"---> UNLOCKING cart items .....");

            return await _httpClient.SendAsync(_request);
        }

    }
}
