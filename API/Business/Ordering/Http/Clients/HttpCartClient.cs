using Business.Ordering.DTOs;
using Business.Ordering.Http.Clients.Interfaces;
using Business.Scheduler.DTOs;
using Business.Scheduler.JWT.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Ordering.Http.Clients
{
    public class HttpCartClient : IHttpCartClient
    {

        private HttpRequestMessage _request;
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
        }






        public async Task<HttpResponseMessage> CreateCart(int userId)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Post,
                $""
            );

            Console.WriteLine($"---> CREATING cart for user '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> ExistsCartByCartId(Guid cartId)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/exists",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartId }), _encoding, _mediaType)
            );

            Console.WriteLine($"---> EXISTS cart '{cartId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Delete,
                $"/items/expired",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartItemLocks }), _encoding, _mediaType)
            );

            Console.WriteLine($"---> REMOVING expired items from carts ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> DeleteCart(int id)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Delete,
                $"/{id}"
            );

            Console.WriteLine($"---> DELETING cart ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetCards()
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/all"
            );

            Console.WriteLine($"---> GETTING carts ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetCartByUserId(int userId)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/{userId}"
            );

            Console.WriteLine($"---> GETTING cart '{userId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> UpdateCart(int userId, CartUpdateDTO cartUpdateDTO)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Put,
                $"/{userId}",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartUpdateDTO }), _encoding, _mediaType)
            );

            Console.WriteLine($"---> UPDATING cart '{userId}' ....");

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
