using Business.Ordering.DTOs;
using Business.Ordering.Http.Interfaces;
using Business.Scheduler.DTOs;
using Business.Scheduler.JWT.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Ordering.Http
{
    public class HttpCartItemClient : IHttpCartItemClient
    {

        private HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IJWTTokenStore _jwtTokenStore;
        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ??
            (_jwtTokenStore.IsExipred ? "" : _jwtTokenStore.Token) ?? "";

        public HttpCartItemClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor, IJWTTokenStore jwtTokenStore)
        {
            _jwtTokenStore = jwtTokenStore;
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:OrderingService").Value + "/api/cart";
            _accessor = accessor;
        }




        public async Task<HttpResponseMessage> AddItemsToCart(int userId, IEnumerable<CartItemUpdateDTO> items)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Post,
                $"/{userId}/items",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), _encoding, _mediaType)               
            );


            Console.WriteLine($"---> ADDING items to cart '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> DeleteExpiredCartItems(IEnumerable<CartItemsLockDeleteDTO> items)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Delete,
                $"/items/expired",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), _encoding, _mediaType)
            );

            Console.WriteLine($"---> DELETING expired cart items ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> DeleteCartItems(int userId, IEnumerable<int> items)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Delete,
                $"/{userId}/items/delete",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), _encoding, _mediaType)
            );

            Console.WriteLine($"---> DELETING items from cart '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetAllCardItems()
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/items/all"

            );

            Console.WriteLine($"---> GETTING all cart items ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetCartItems(int userId)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/{userId}/items"
            );

            Console.WriteLine($"---> GETTING cart items for cart '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> RemoveCartItems(int cartId, IEnumerable<CartItemUpdateDTO> items)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Delete,
                $"/{cartId}/items",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), _encoding, _mediaType)
            );

            Console.WriteLine($"---> REMOVING cart items from cart '{cartId}' ....");

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
