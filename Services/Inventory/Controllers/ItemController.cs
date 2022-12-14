using Business.Inventory.DTOs.Item;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Services.Inventory.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<ActionResult> GetItems(IEnumerable<int> itemIds)
        {
            var result = await _itemService.GetItems(itemIds);

            return Ok(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}", Name = "GetItemById")]
        public async Task<ActionResult> GetItemById(int id)
        {
            var result = await _itemService.GetItemById(id);

            return Ok(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost]
        public async Task<ActionResult> AddItem(ItemCreateDTO itemCreateDTO)
        {
           var result = await _itemService.AddItem(itemCreateDTO);

            return Ok(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, ItemUpdateDTO itemUpdateDTO)
        {
            var result = await _itemService.UpdateItem(id, itemUpdateDTO);

            return Ok(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemById(int id)
        {
            var result = await _itemService.DeleteItem(id);

            return Ok(result);
        }

    }
}
