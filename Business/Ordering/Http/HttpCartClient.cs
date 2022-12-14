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
    public class HttpCartClient : IHttpCartClient
    {
        private static IJWTTokenStore _jwtTokenStore;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private static IHttpContextAccessor _accessor;

        private static string _token => _accessor == null ? "" :
            _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ??
            (_jwtTokenStore.IsExipred ? "" : _jwtTokenStore.Token) ?? 
            "";

        public HttpCartClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor, IJWTTokenStore jwtTokenStore)
        {
            _jwtTokenStore = jwtTokenStore;
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:OrderingService").Value + "/api/cart";
            _accessor = accessor;
        }






        public async Task<HttpResponseMessage> CreateCart(int userId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUri}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> CREATING cart for user '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> ExistsCartByCartId(Guid cartId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/exists"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartId }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> EXISTS cart '{cartId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/items/expired"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartItemLocks }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> REMOVING expired items from carts ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> DeleteCart(int id)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUri}/{id}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> DELETING cart ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetCards()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/all")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING carts ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> GetCartByUserId(int userId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri}/{userId}")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> GETTING cart '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }



        public async Task<HttpResponseMessage> UpdateCart(int userId, CartUpdateDTO cartUpdateDTO)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUri}/{userId}"),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartUpdateDTO }), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> UPDATING cart '{userId}' ....");

            return await _httpClient.SendAsync(request);
        }
    }
}
