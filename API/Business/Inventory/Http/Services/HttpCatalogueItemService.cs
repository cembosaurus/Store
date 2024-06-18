using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;



namespace Business.Inventory.Http.Services
{
    public class HttpCatalogueItemService : HttpBaseService, IHttpCatalogueItemService
    {

        public HttpCatalogueItemService(IHostingEnvironment env, IExId exId, IAppsettings_PROVIDER appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IGlobal_Settings_PROVIDER remoteServices_Provider)
            : base(env, exId, appsettingsService, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "InventoryService";
            _remoteServicePathName = "CatalogueItem";
        }





        public async Task<IServiceResult<ExtrasReadDTO>> AddExtrasToCatalogueItem(int itemId, ExtrasAddDTO extrasAddDTO)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"{itemId}/extras";
            _content = new StringContent(JsonConvert.SerializeObject(extrasAddDTO), _encoding, _mediaType);

            return await HTTP_Request_Handler<ExtrasReadDTO>();
        }



        public async Task<IServiceResult<int>> AddAmountToStock(int itemId, int amount)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"{itemId}/tostock/{amount}";

            return await HTTP_Request_Handler<int>();
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItemCreateDTO)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"{itemId}";
            _content = new StringContent(JsonConvert.SerializeObject(catalogueItemCreateDTO), _encoding, _mediaType);

            return await HTTP_Request_Handler<CatalogueItemReadDTO>();
        }



        public async Task<IServiceResult<bool>> ExistsCatalogueItemById(int itemId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{itemId}/exists";

            return await HTTP_Request_Handler<bool>();
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> GetCatalogueItemById(int itemId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{itemId}";

            return await HTTP_Request_Handler<CatalogueItemReadDTO>();
        }



        public async Task<IServiceResult<IEnumerable<CatalogueItemReadDTO>>> GetCatalogueItems(IEnumerable<int> itemIds = null)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{(itemIds != null && itemIds.Any() ? "" : "all")}";
            _content = new StringContent(JsonConvert.SerializeObject(itemIds), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<CatalogueItemReadDTO>>();
        }



        public async Task<IServiceResult<CatalogueItemReadDTOWithExtras>> GetCatalogueItemWithExtrasById(int itemId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{itemId}/extras";

            return await HTTP_Request_Handler<CatalogueItemReadDTOWithExtras>();
        }



        public async Task<IServiceResult<int>> GetInstockCount(int itemId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{itemId}/instock";

            return await HTTP_Request_Handler<int>();
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> RemoveCatalogueItem(int itemId)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"{itemId}";

            return await HTTP_Request_Handler<CatalogueItemReadDTO>();
        }



        public async Task<IServiceResult<ExtrasReadDTO>> RemoveExtrasFromCatalogueItem(int itemId, ExtrasRemoveDTO extrasRemoveDTO)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"{itemId}/extras";
            _content = new StringContent(JsonConvert.SerializeObject(extrasRemoveDTO), _encoding, _mediaType);

            return await HTTP_Request_Handler<ExtrasReadDTO>();
        }



        public async Task<IServiceResult<int>> RemoveAmountFromStock(int itemId, int amount)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"{itemId}/fromstock/{amount}";

            return await HTTP_Request_Handler<int>();
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO catalogueItemUpdateDTO)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"{itemId}";
            _content = new StringContent(JsonConvert.SerializeObject(catalogueItemUpdateDTO), _encoding, _mediaType);

            return await HTTP_Request_Handler<CatalogueItemReadDTO>();
        }
    }
}
