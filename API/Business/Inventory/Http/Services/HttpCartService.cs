using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services;
using Business.Http.Services.Interfaces;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Ordering.DTOs;
using Business.Scheduler.DTOs;
using Business.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;



namespace Business.Inventory.Http.Services
{
    public class HttpCartService : HttpBaseService, IHttpBaseService, IHttpCartService
    {

        public HttpCartService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact, ConsoleWriter cm)
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact, cm)
        {
            _remoteServiceName = "OrderingService";
            _remoteServicePathName = "Cart";
        }




        public async Task<IServiceResult<CartReadDTO>> CreateCart(int userId)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"";

            return await HTTP_Request_Handler<CartReadDTO>();
        }

        public async Task<IServiceResult<CartReadDTO>> DeleteCart(int id)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"{id}";

            return await HTTP_Request_Handler<CartReadDTO>();
        }

        public async Task<IServiceResult<bool>> ExistsCartByCartId(Guid cartId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"exists";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartId }), _encoding, _mediaType);

            return await HTTP_Request_Handler<bool>();
        }

        public async Task<IServiceResult<IEnumerable<CartReadDTO>>> GetCards()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"all";

            return await HTTP_Request_Handler<IEnumerable<CartReadDTO>>();
        }

        public async Task<IServiceResult<CartReadDTO>> GetCartByUserId(int userId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{userId}";

            return await HTTP_Request_Handler<CartReadDTO>();
        }

        public async Task<IServiceResult<CartReadDTO>> UpdateCart(int userId, CartUpdateDTO cartUpdateDTO)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"{userId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartUpdateDTO }), _encoding, _mediaType);

            return await HTTP_Request_Handler<CartReadDTO>();
        }


        // request initiated by API services (Scheduler Service), NOT by users:
        public async Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> RemoveExpiredItemsFromCart(IEnumerable<CartItemsLockDeleteDTO> cartItemLocks)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"items/expired";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { cartItemLocks }), _encoding, _mediaType);

            _useApiKey = true;

            return await HTTP_Request_Handler<IEnumerable<CartItemsLockReadDTO>>();
        }
    }
}
