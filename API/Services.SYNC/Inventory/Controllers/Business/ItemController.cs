using Business.Inventory.DTOs.Item;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers.Business
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        private readonly IItemService _itemService;


        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }




        [Authorize(Policy = "Everyone")]
        [HttpGet("all")]
        public async Task<object> GetAllItems()
        {
            var result = await _itemService.GetItems();

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<object> GetItems(IEnumerable<int> itemIds)
        {
            var result = await _itemService.GetItems(itemIds);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}", Name = "GetItemById")]
        public async Task<object> GetItemById(int id)
        {
            var result = await _itemService.GetItemById(id);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost]
        public async Task<object> AddItem(ItemCreateDTO itemCreateDTO)
        {
            var result = await _itemService.AddItem(itemCreateDTO);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{id}")]
        public async Task<object> UpdateItem(int id, ItemUpdateDTO itemUpdateDTO)
        {
            var result = await _itemService.UpdateItem(id, itemUpdateDTO);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpDelete("{id}")]
        public async Task<object> DeleteItemById(int id)
        {
            var result = await _itemService.DeleteItem(id);

            return result;  // ctr res
        }


    }
}
