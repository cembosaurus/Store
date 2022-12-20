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
        private static IJWTTokenStore _jwtTokenStore;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private static IHttpContextAccessor _accessor;

        private static string _token => _accessor == null ? "" :
            _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ??
            (_jwtTokenStore.IsExipred ? "" : _jwtTokenStore.Token) ??
            "";

        public HttpCartItemClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor, IJWTTokenStore jwtTokenStore)
        {
            _jwtTokenStore = jwtTokenStore;
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:OrderingService").Value + "/api/cart";
            _accessor = accessor;
        }




        public async Task<HttpResponseMessage> AddItemsToCart(int userId, IEnumerable<CartItemUpdateDTO> items)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}/{userId}/items"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> ADDING items to cart '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> DeleteExpiredCartItems(IEnumerable<CartItemsLockDeleteDTO> items)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/items/expired"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> DELETING expired cart items ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> DeleteCartItems(int userId, IEnumerable<int> items)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/{userId}/items/delete"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> DELETING items from cart '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetAllCardItems()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/items/all")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING all cart items ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetCartItems(int userId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/{userId}/items")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING cart items for cart '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> RemoveCartItems(int cartId, IEnumerable<CartItemUpdateDTO> items)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/{cartId}/items"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> REMOVING cart items from cart '{cartId}' ....");

            return await _httpClient.SendAsync(request);
        }
    }
}
