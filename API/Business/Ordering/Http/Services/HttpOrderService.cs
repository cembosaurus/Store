using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Services;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Ordering.DTOs;
using Business.Ordering.Http.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Business.Ordering.Http.Services
{
    public class HttpOrderService : HttpBaseService, IHttpOrderService
    {


        public HttpOrderService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact)
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "OrderingService";
            _remoteServicePathName = "Order";
        }





        public async Task<IServiceResult<OrderReadDTO>> CompleteOrder(int userId)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"{userId}/complete";

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<OrderReadDTO>> CreateOrder(int userId, OrderCreateDTO orderCreateDTO)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"{userId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderCreateDTO }), _encoding, _mediaType);

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<OrderReadDTO>> DeleteOrder(int userId)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"{userId}";

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<IEnumerable<OrderReadDTO>>> GetAllOrders()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"all";

            return await HTTP_Request_Handler<IEnumerable<OrderReadDTO>>();
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByCartId(Guid cartId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"cart";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartId }), _encoding, _mediaType);

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByOrderCode(string orderCode)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"ordercode";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderCode }), _encoding, _mediaType);

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByUserId(int userId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"user/{userId}";

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<OrderReadDTO>> UpdateOrder(int userId, OrderUpdateDTO orderUpdateDTO)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"{userId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderUpdateDTO }), _encoding, _mediaType);

            return await HTTP_Request_Handler<OrderReadDTO>();
        }
    }
}
