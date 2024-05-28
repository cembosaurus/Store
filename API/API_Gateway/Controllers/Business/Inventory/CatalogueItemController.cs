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
        public async Task<ActionResult> GetAllCatalogueItems()
        {
            var result = await _catalogueItemService.GetCatalogueItems();

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<ActionResult> GetCatalogueItems(IEnumerable<int> itemIds)
        {
            var result = await _catalogueItemService.GetCatalogueItems(itemIds);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}")]
        public async Task<ActionResult> GetCatalogueItemById(int itemId)
        {
            var result = await _catalogueItemService.GetCatalogueItemById(itemId);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}/extras")]
        public async Task<ActionResult> GetCatalogueItemWithExtrasById(int itemId)
        {
            var result = await _catalogueItemService.GetCatalogueItemWithExtrasById(itemId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}/instock")]
        public async Task<ActionResult> GetInstockCount(int itemId)
        {
            var result = await _catalogueItemService.GetInstockCount(itemId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        // POST:

        [Authorize(Policy = "Everyone")]
        [HttpPost("{itemId}")]
        public async Task<ActionResult> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItem)
        {
            var result = await _catalogueItemService.CreateCatalogueItem(itemId, catalogueItem);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        [Authorize(Policy = "Everyone")]
        [HttpPost("{itemId}/extras")]
        public async Task<ActionResult> AddExtrasToCatalogueItem(int itemId, ExtrasAddDTO extras)
        {
            var result = await _catalogueItemService.AddExtrasToCatalogueItem(itemId, extras);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        // PUT:

        [Authorize(Policy = "Everyone")]
        [HttpPut("{itemId}")]
        public async Task<ActionResult> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO cataloguaItem)
        {
            var result = await _catalogueItemService.UpdateCatalogueItem(itemId, cataloguaItem);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpPut("{itemId}/tostock/{amount}")]
        public async Task<ActionResult> AddAmountToStock(int itemId, int amount)
        {
            var result = await _catalogueItemService.AddAmountToStock(itemId, amount);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        // DELETE:

        [Authorize(Policy = "Everyone")]
        [HttpDelete("{itemId}")]
        public async Task<ActionResult> RemoveCatalogueItem(int itemId)
        {
            var result = await _catalogueItemService.RemoveCatalogueItem(itemId);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpDelete("{itemId}/extras")]
        public async Task<ActionResult> RemoveExtrasFromCatalogueItem(int itemId, ExtrasRemoveDTO extras)
        {
            var result = await _catalogueItemService.RemoveExtrasFromCatalogueItem(itemId, extras);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpDelete("{itemId}/fromstock/{amount}")]
        public async Task<ActionResult> RemoveFromStockAmount(int itemId, int amount)
        {
            var result = await _catalogueItemService.RemoveFromStockAmount(itemId, amount);

            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
