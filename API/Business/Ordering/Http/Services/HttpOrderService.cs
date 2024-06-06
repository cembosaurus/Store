using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Ordering.DTOs;
using Business.Ordering.Http.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;



namespace Business.Ordering.Http.Services
{
    public class HttpOrderService : HttpBaseService, IHttpOrderService
    {


        public HttpOrderService(IHostingEnvironment env, IExId exId, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IRemoteServicesInfo_Provider remoteServicesInfoService)
            : base(env, exId, appsettingsService, httpAppClient, remoteServicesInfoService, resultFact)
        {
            _remoteServiceName = "OrderingService";
            _remoteServicePathName = "Order";
        }





        public async Task<IServiceResult<OrderReadDTO>> CompleteOrder(int userId)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"/{userId}/complete";

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<OrderReadDTO>> CreateOrder(int userId, OrderCreateDTO orderCreateDTO)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/{userId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderCreateDTO }), _encoding, _mediaType);

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<OrderReadDTO>> DeleteOrder(int userId)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"/{userId}";

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<IEnumerable<OrderReadDTO>>> GetAllOrders()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/all";

            return await HTTP_Request_Handler<IEnumerable<OrderReadDTO>>();
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByCartId(Guid cartId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/cart";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartId }), _encoding, _mediaType);

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByOrderCode(string orderCode)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/ordercode";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderCode }), _encoding, _mediaType);

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByUserId(int userId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/user/{userId}";

            return await HTTP_Request_Handler<OrderReadDTO>();
        }



        public async Task<IServiceResult<OrderReadDTO>> UpdateOrder(int userId, OrderUpdateDTO orderUpdateDTO)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"/{userId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { orderUpdateDTO }), _encoding, _mediaType);

            return await HTTP_Request_Handler<OrderReadDTO>();
        }
    }
}
