using Business.Ordering.DTOs;
using Business.Ordering.Http.Clients.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Ordering.Http.Clients
{
    public class HttpOrderClient : IHttpOrderClient
    {

        private HttpRequestMessage _request;
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
        }




        public async Task<HttpResponseMessage> CompleteOrder(int userId)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Put,
                $"/{userId}/complete"
            );

            Console.WriteLine($"---> COMPLETING order '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> CreateOrder(int userId, OrderCreateDTO orderDTO)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Post,
                $"/{userId}",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderDTO }), _encoding, _mediaType)
            );

            Console.WriteLine($"---> CREATING order '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> DeleteOrder(int userId)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Delete,
                $"/{userId}"
            );

            Console.WriteLine($"---> DELETING order '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetAllOrders()
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/all"
            );

            Console.WriteLine($"---> GETTING all orders ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetOrderByCartId(Guid cartId)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/cart",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartId }), _encoding, _mediaType)
            );

            Console.WriteLine($"---> GETTING order '{cartId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetOrderByOrderCode(string code)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/ordercode",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { code }), _encoding, _mediaType)
            );

            Console.WriteLine($"---> GETTING order '{code}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetOrderByUserId(int userId)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/user/{userId}"
            );

            Console.WriteLine($"---> GETTING order '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> UpdateOrder(int userId, OrderUpdateDTO orderDTO)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Put,
                $"/{userId}",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderDTO }), _encoding, _mediaType)
            );

            Console.WriteLine($"---> UPDATING order '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }




        private void InitializeHttpRequestMessage(HttpMethod method, string uri, HttpContent content = default)
        {
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri + uri) };
            _request.Method = method;
            _request.Content = content;
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
    }
}
