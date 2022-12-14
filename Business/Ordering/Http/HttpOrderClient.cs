using Business.Libraries.Http.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Ordering.Http.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

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
            _baseUri = config.GetSection("RemoteServices:OrderingService").Value + "/api";
            _accessor = accessor;
        }





        //........................................................... Change result type to HttpResponseMessage as in other http clients
        public Task<IEnumerable<OrderReadDTO>> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public Task<OrderReadDTO> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderReadDTO> AddOrder(OrderReadDTO orderDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrder(OrderReadDTO orderDTO)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
