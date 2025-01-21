using Business.Inventory.DTOs.ItemPrice;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers.Business
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
        [HttpGet("{itemId}", Name = "GetItemPriceById")]
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
