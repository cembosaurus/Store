using Business.Exceptions.Interfaces;
using Business.Http.Clients;
using Business.Http.Services;
using Business.Inventory.DTOs.ItemPrice;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;



namespace Business.Inventory.Http.Services
{
    public class HttpItemPriceService : HttpBaseService, IHttpItemPriceService
    {

        public HttpItemPriceService(IWebHostEnvironment env, IExId exId, IAppsettings_PROVIDER appsettingsService, IHttpAppClient httpAppClient, IGlobal_Settings_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact)
            : base(env, exId, appsettingsService, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "InventoryService";
            _remoteServicePathName = "ItemPrice";
        }





        public async Task<IServiceResult<IEnumerable<ItemPriceReadDTO>>> GetItemPrices(IEnumerable<int> itemIds = default)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{(itemIds != null && itemIds.Any() ? "" : "all")}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemIds), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<ItemPriceReadDTO>>();
        }


        public async Task<IServiceResult<ItemPriceReadDTO>> GetItemPriceById(int itemId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{itemId}";

            return await HTTP_Request_Handler<ItemPriceReadDTO>();
        }


        public async Task<IServiceResult<ItemPriceReadDTO>> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceUpdateDTO)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"{itemId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemPriceUpdateDTO), _encoding, _mediaType);

            return await HTTP_Request_Handler<ItemPriceReadDTO>();
        }
    }
}
