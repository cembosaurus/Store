using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Ordering.Http.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Ordering.Http.temp
{
    public class HttpOrderClient : IHttpOrderClient
    {

        private readonly HttpClient _httpClient;
        private readonly IServiceResultFactory _resultFact;
        private readonly object _baseUri;
        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";


        public HttpOrderClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor, IServiceResultFactory resultFact)
        {
            _httpClient = httpClient;
            _resultFact = resultFact;
            _baseUri = config.GetSection("RemoteServices:OrderingService").Value + "/api/order";
            _accessor = accessor;
        }



        
        public async Task<HttpResponseMessage> CompleteOrder(int userId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUri}/{userId}/complete")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> COMPLETING order '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> CreateOrder(int userId, OrderCreateDTO orderDTO)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}/{userId}"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderDTO }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> CREATING order '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> DeleteOrder(int userId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/{userId}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> DELETING order '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetAllOrders()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/all")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING all orders ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetOrderByCartId(Guid cartId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/cart"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartId }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING order '{cartId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetOrderByOrderCode(string code)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/ordercode"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { code }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING order '{code}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetOrderByUserId(int userId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/user/{userId}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING order '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> UpdateOrder(int userId, OrderUpdateDTO orderDTO)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUri}/{userId}"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderDTO }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> UPDATING order '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }
    }
}
