using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services;
using Business.Inventory.DTOs.ItemPrice;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Business.Inventory.Http.Services
{
    public class HttpItemPriceService : HttpBaseService, IHttpItemPriceService
    {

        public HttpItemPriceService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact, ConsoleWriter cm) 
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact, cm)
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
