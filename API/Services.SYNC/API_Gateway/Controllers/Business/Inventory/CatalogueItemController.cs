using API_Gateway.Services.Business.Inventory.Interfaces;
using Business.Inventory.DTOs.CatalogueItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Business.Inventory
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CatalogueItemController : ControllerBase
    {

        private readonly ICatalogueItemService _catalogueItemService;

        public CatalogueItemController(ICatalogueItemService catalogueItemService)
        {
            _catalogueItemService = catalogueItemService;
        }






        // GET:
        [Authorize(Policy = "Everyone")]
        [HttpGet("all")]
        public async Task<object> GetAllCatalogueItems()
        {
            var result = await _catalogueItemService.GetCatalogueItems();

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<object> GetCatalogueItems(IEnumerable<int> itemIds)
        {
            var result = await _catalogueItemService.GetCatalogueItems(itemIds);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}")]
        public async Task<object> GetCatalogueItemById(int itemId)
        {
            var result = await _catalogueItemService.GetCatalogueItemById(itemId);

            return result;  // ctr res
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}/extras")]
        public async Task<object> GetCatalogueItemWithExtrasById(int itemId)
        {
            var result = await _catalogueItemService.GetCatalogueItemWithExtrasById(itemId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}/instock")]
        public async Task<object> GetInstockCount(int itemId)
        {
            var result = await _catalogueItemService.GetInstockCount(itemId);

            return result;  // ctr res
        }



        // POST:

        [Authorize(Policy = "Everyone")]
        [HttpPost("{itemId}")]
        public async Task<object> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItem)
        {
            var result = await _catalogueItemService.CreateCatalogueItem(itemId, catalogueItem);

            return result;  // ctr res
        }




        [Authorize(Policy = "Everyone")]
        [HttpPost("{itemId}/extras")]
        public async Task<object> AddExtrasToCatalogueItem(int itemId, ExtrasAddDTO extras)
        {
            var result = await _catalogueItemService.AddExtrasToCatalogueItem(itemId, extras);

            return result;  // ctr res
        }



        // PUT:

        [Authorize(Policy = "Everyone")]
        [HttpPut("{itemId}")]
        public async Task<object> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO cataloguaItem)
        {
            var result = await _catalogueItemService.UpdateCatalogueItem(itemId, cataloguaItem);

            return result;  // ctr res
        }


        [Authorize(Policy = "Everyone")]
        [HttpPut("{itemId}/tostock/{amount}")]
        public async Task<object> AddAmountToStock(int itemId, int amount)
        {
            var result = await _catalogueItemService.AddAmountToStock(itemId, amount);

            return result;  // ctr res
        }



        // DELETE:

        [Authorize(Policy = "Everyone")]
        [HttpDelete("{itemId}")]
        public async Task<object> RemoveCatalogueItem(int itemId)
        {
            var result = await _catalogueItemService.RemoveCatalogueItem(itemId);

            return result;  // ctr res
        }


        [Authorize(Policy = "Everyone")]
        [HttpDelete("{itemId}/extras")]
        public async Task<object> RemoveExtrasFromCatalogueItem(int itemId, ExtrasRemoveDTO extras)
        {
            var result = await _catalogueItemService.RemoveExtrasFromCatalogueItem(itemId, extras);

            return result;  // ctr res
        }


        [Authorize(Policy = "Everyone")]
        [HttpDelete("{itemId}/fromstock/{amount}")]
        public async Task<object> RemoveFromStockAmount(int itemId, int amount)
        {
            var result = await _catalogueItemService.RemoveFromStockAmount(itemId, amount);

            return result;  // ctr res
        }
    }
}
