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

        private readonly HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";


        public HttpOrderClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:OrderingService").Value + "/api/order";
            _accessor = accessor;
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri) };
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }




        public async Task<HttpResponseMessage> CompleteOrder(int userId)
        {
            _request.Method = HttpMethod.Put;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{userId}/complete");

            Console.WriteLine($"---> COMPLETING order '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> CreateOrder(int userId, OrderCreateDTO orderDTO)
        {
            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{userId}");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderDTO }), _encoding, _mediaType);

            Console.WriteLine($"---> CREATING order '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> DeleteOrder(int userId)
        {
            _request.Method = HttpMethod.Delete;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{userId}");

            Console.WriteLine($"---> DELETING order '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetAllOrders()
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/all");

            Console.WriteLine($"---> GETTING all orders ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetOrderByCartId(Guid cartId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/cart");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartId }), _encoding, _mediaType);

            Console.WriteLine($"---> GETTING order '{cartId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetOrderByOrderCode(string code)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/ordercode");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { code }), _encoding, _mediaType);

            Console.WriteLine($"---> GETTING order '{code}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetOrderByUserId(int userId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/user/{userId}");

            Console.WriteLine($"---> GETTING order '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> UpdateOrder(int userId, OrderUpdateDTO orderDTO)
        {
            _request.Method = HttpMethod.Put;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{userId}");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderDTO }), _encoding, _mediaType);

            Console.WriteLine($"---> UPDATING order '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }
    }
}
