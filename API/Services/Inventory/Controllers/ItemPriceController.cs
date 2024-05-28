using Business.Inventory.DTOs.ItemPrice;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Services.Inventory.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ItemPriceController : ControllerBase
    {

        private readonly IItemPriceService _itemPriceService;


        public ItemPriceController(IItemPriceService itemPriceservice)
        {
            _itemPriceService = itemPriceservice;
        }






        [Authorize(Policy = "Everyone")]
        [HttpGet("all")]
        public async Task<ActionResult> GetAllItemPrices()
        {
            var result = await _itemPriceService.GetItemPrices();

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<ActionResult> GetItemPrices(IEnumerable<int> itemIds)
        {
            var result = await _itemPriceService.GetItemPrices(itemIds);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}", Name = "GetItemPriceById")]
        public async Task<ActionResult> GetItemPriceById(int itemId)
        {
            var result = await _itemPriceService.GetItemPriceById(itemId);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        [Authorize(Policy = "Everyone")]
        [HttpPut("{itemId}")]
        public async Task<ActionResult> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceEditDTO)
        {
            var result = await _itemPriceService.UpdateItemPrice(itemId, itemPriceEditDTO);

            return result.Status ? Ok(result) : BadRequest(result);
        }

    }
}
