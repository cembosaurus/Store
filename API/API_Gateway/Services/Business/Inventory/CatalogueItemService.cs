using API_Gateway.Services.Business.Inventory.Interfaces;
using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;



namespace API_Gateway.Services.Business.Inventory
{
    public class CatalogueItemService : ICatalogueItemService
    {

        private readonly IHttpCatalogueItemService _httpCatalogueItemService;


        public CatalogueItemService(IHttpCatalogueItemService httpCatalogueItemService)
        {
            _httpCatalogueItemService = httpCatalogueItemService;
        }




        public async Task<IServiceResult<ExtrasReadDTO>> AddExtrasToCatalogueItem(int id, ExtrasAddDTO extrasAddDTO)
        {
            return await _httpCatalogueItemService.AddExtrasToCatalogueItem(id, extrasAddDTO);
        }



        public async Task<IServiceResult<int>> AddAmountToStock(int itemId, int amount)
        {
            return await _httpCatalogueItemService.AddAmountToStock(itemId, amount);
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItemCreateDTO)
        {
            return await _httpCatalogueItemService.CreateCatalogueItem(itemId, catalogueItemCreateDTO);
        }



        public async Task<IServiceResult<bool>> ExistsCatalogueItemById(int id)
        {
            return await _httpCatalogueItemService.ExistsCatalogueItemById(id);
        }

        public async Task<IServiceResult<CatalogueItemReadDTO>> GetCatalogueItemById(int id)
        {
            return await _httpCatalogueItemService.GetCatalogueItemById(id);
        }



        public async Task<IServiceResult<IEnumerable<CatalogueItemReadDTO>>> GetCatalogueItems(IEnumerable<int> itemIds = null)
        {
            return await _httpCatalogueItemService.GetCatalogueItems(itemIds);
        }

        public async Task<IServiceResult<CatalogueItemReadDTOWithExtras>> GetCatalogueItemWithExtrasById(int id)
        {
            return await _httpCatalogueItemService.GetCatalogueItemWithExtrasById(id);
        }



        public async Task<IServiceResult<int>> GetInstockCount(int id)
        {
            return await _httpCatalogueItemService.GetInstockCount(id);
        }

        public async Task<IServiceResult<CatalogueItemReadDTO>> RemoveCatalogueItem(int id)
        {
            return await _httpCatalogueItemService.RemoveCatalogueItem(id);
        }



        public async Task<IServiceResult<ExtrasReadDTO>> RemoveExtrasFromCatalogueItem(int id, ExtrasRemoveDTO extrasRemoveDTO)
        {
            return await _httpCatalogueItemService.RemoveExtrasFromCatalogueItem(id, extrasRemoveDTO);
        }



        public async Task<IServiceResult<int>> RemoveFromStockAmount(int itemId, int amount)
        {
            return await _httpCatalogueItemService.RemoveFromStockAmount(itemId, amount);
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO catalogueItemUpdateDTO)
        {
            return await _httpCatalogueItemService.UpdateCatalogueItem(itemId, catalogueItemUpdateDTO);
        }
    }
}
