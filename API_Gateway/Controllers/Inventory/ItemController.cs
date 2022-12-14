using API_Gateway.Services.Inventory;
using Business.Inventory.DTOs.Item;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Inventory
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        private readonly string _url;
        private readonly ItemService _itemService;

        public ItemController(IConfiguration conf, ItemService itemService)
        {
            _url = conf.GetSection("RemoteServices:InventoryService").Value + "/api/Item";
            _itemService = itemService;
        }





        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<ActionResult> GetAllItems()
        {
            var result = await _itemService.GetItems();

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
        public async Task<ActionResult> DeleteItem(int id)
        {
            var result = await _itemService.DeleteItem(id);

            return result.Status ? Ok(result) : BadRequest(result);
        }





    }
}
