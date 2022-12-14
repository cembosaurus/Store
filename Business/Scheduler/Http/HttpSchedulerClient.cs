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

        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";

        public HttpSchedulerClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:SchedulerService").Value + "/api/scheduler";
            _accessor = accessor;
        }





        public async Task<HttpResponseMessage> CartItemsLock(CartItemsLockCreateDTO cartItemsToLock)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}/cartitem/lock"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(cartItemsToLock), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> LOCKING cart items .....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> CartItemsUnLock(CartItemsLockDeleteDTO cartItemsToUnLock)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/cartitem/lock"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(cartItemsToUnLock), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> UNLOCKING cart items .....");

            return await _httpClient.SendAsync(request);
        }

    }
}
