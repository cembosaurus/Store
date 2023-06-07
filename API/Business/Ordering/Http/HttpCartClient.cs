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

        private readonly HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IJWTTokenStore _jwtTokenStore;
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
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri) };
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }






        public async Task<HttpResponseMessage> CreateCart(int userId)
        {
            _request.Method = HttpMethod.Post;

            Console.WriteLine($"---> CREATING cart for user '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> ExistsCartByCartId(Guid cartId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/exists");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartId }), _encoding, _mediaType);

            Console.WriteLine($"---> EXISTS cart '{cartId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks)
        {
            _request.Method = HttpMethod.Delete;
            _request.RequestUri = new Uri(_request.RequestUri + $"/items/expired");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartItemLocks }), _encoding, _mediaType);

            Console.WriteLine($"---> REMOVING expired items from carts ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> DeleteCart(int id)
        {
            _request.Method = HttpMethod.Delete;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{id}");

            Console.WriteLine($"---> DELETING cart ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetCards()
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/all");

            Console.WriteLine($"---> GETTING carts ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetCartByUserId(int userId)
        {
            _request.Method = HttpMethod.Get;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{userId}");

            Console.WriteLine($"---> GETTING cart '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> UpdateCart(int userId, CartUpdateDTO cartUpdateDTO)
        {
            _request.Method = HttpMethod.Put;
            _request.RequestUri = new Uri(_request.RequestUri + $"/{userId}");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartUpdateDTO }), _encoding, _mediaType);

            Console.WriteLine($"---> UPDATING cart '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }
    }
}
