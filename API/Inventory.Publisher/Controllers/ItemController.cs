using Business.Inventory.Controllers.Interfaces;
using Business.Inventory.DTOs.Item;
using Inventory.Publisher.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Publisher.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ItemController : ControllerBase, IItemController
    {

        private readonly IItemService _itemService;


        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }




        [Authorize(Policy = "Everyone")]
        [HttpGet("all")]
        public async Task<ActionResult> GetAllItems()
        {
            var result = await _itemService.GetItems();

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<ActionResult> GetItems(IEnumerable<int> itemIds)
        {
            var result = await _itemService.GetItems(itemIds);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetItemById(int id)
        {
            var result = await _itemService.GetItemById(id);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost]
        public async Task<ActionResult> AddItem(ItemCreateDTO item)
        {
            var result = await _itemService.AddItem(item);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, ItemUpdateDTO item)
        {
            var result = await _itemService.UpdateItem(id, item);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemById(int id)
        {
            var result = await _itemService.DeleteItem(id);

            return result.Status ? Ok(result) : BadRequest(result);
        }





    }
}
