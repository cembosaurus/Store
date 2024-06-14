using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
using Business.Inventory.DTOs.Item;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;



namespace Business.Inventory.Http.Services
{
    public class HttpItemService : HttpBaseService, IHttpItemService
    {


        public HttpItemService(IHostingEnvironment env, IExId exId, IAppsettings_PROVIDER appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IRemoteServices_PROVIDER remoteServices_Provider)
            : base(env, exId, appsettingsService, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "InventoryService";
            _remoteServicePathName = "Item";
        }





        public async Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = default)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{(itemIds != null && itemIds.Any() ? "" : "/all")}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemIds), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<ItemReadDTO>>();
        }



        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int itemId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/{itemId}";

            return await HTTP_Request_Handler<ItemReadDTO>();
        }



        public async Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO itemDTO)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemDTO), _encoding, _mediaType);

            return await HTTP_Request_Handler<ItemReadDTO>();
        }



        public async Task<IServiceResult<ItemReadDTO>> DeleteItem(int itemId)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"{_requestURL}/{itemId}";

            return await HTTP_Request_Handler<ItemReadDTO>();
        }



        public async Task<IServiceResult<ItemReadDTO>> UpdateItem(int itemId, ItemUpdateDTO itemDTO)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"{_requestURL}/{itemId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemDTO), _encoding, _mediaType);

            return await HTTP_Request_Handler<ItemReadDTO>();
        }
    }
}
