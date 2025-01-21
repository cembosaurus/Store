using API_Gateway.Services.Business.Inventory.Interfaces;
using Business.Inventory.DTOs.ItemPrice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Business.Inventory
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ItemPriceController : ControllerBase
    {

        private readonly IItemPriceService _itemPriceService;

        public ItemPriceController(IItemPriceService itemPriceService)
        {
            _itemPriceService = itemPriceService;
        }





        [Authorize(Policy = "Everyone")]
        [HttpGet("all")]
        public async Task<object> GetAllItemPrices()
        {
            var result = await _itemPriceService.GetItemPrices();

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<object> GetItemPrices(IEnumerable<int> itemIds)
        {
            var result = await _itemPriceService.GetItemPrices(itemIds);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}")]
        public async Task<object> GetItemPriceById(int itemId)
        {
            var result = await _itemPriceService.GetItemPriceById(itemId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{itemId}")]
        public async Task<object> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceEditDTO)
        {
            var result = await _itemPriceService.UpdateItemPrice(itemId, itemPriceEditDTO);

            return result;  // ctr res
        }

    }
}
