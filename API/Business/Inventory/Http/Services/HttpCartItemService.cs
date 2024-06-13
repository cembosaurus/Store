using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Ordering.DTOs;
using Microsoft.AspNetCore.Hosting;



namespace Business.Inventory.Http.Services
{
    public class HttpCartItemService : HttpBaseService, IHttpCartItemService
    {

        public HttpCartItemService(IHostingEnvironment env, IExId exId, IAppsettings_Provider appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IRemoteServices_Provider remoteServices_Provider)
            : base(env, exId, appsettingsService, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "OrderingService";
            _remoteServicePathName = "Cart";
        }





        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> AddItemsToCart(int userId, IEnumerable<CartItemUpdateDTO> items)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/{userId}/items";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<CartItemReadDTO>>();
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> DeleteCartItems(int userId, IEnumerable<int> items)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"/{userId}/items/delete";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<CartItemReadDTO>>();
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetAllCartsItems()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/items/all";

            return await HTTP_Request_Handler<IEnumerable<CartItemReadDTO>>();
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetCartItems(int userId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/{userId}/items";

            return await HTTP_Request_Handler<IEnumerable<CartItemReadDTO>>();
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> RemoveCartItems(int userId, IEnumerable<CartItemUpdateDTO> items)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"/{userId}/items";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<CartItemReadDTO>>();
        }
    }
}
