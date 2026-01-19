using API_Gateway.Services.Business.Inventory.Interfaces;
using Business.Inventory.DTOs.Item;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Business.Inventory
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ItemController : AppControllerBase
    {

        private readonly IItemService _itemService;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IItemService itemService, ILogger<ItemController> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }






        [Authorize(Policy = "Everyone")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ItemReadDTO>>> GetAllItems()
        {
            // debug:
            _logger.LogInformation("-----> Start fetching all items from inventory.");

            var result = await _itemService.GetItems();

            // debug:
            _logger.LogInformation("-----> ITEMS count: {Count}", result?.Data?.Count() ?? 0);

            return FromServiceResult(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemReadDTO>>> GetItems([FromQuery] IEnumerable<int> itemIds)
        {
            var result = await _itemService.GetItems(itemIds);

            return FromServiceResult(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemReadDTO>> GetItemById(int id)
        {
            var result = await _itemService.GetItemById(id);

            return FromServiceResult(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost]
        public async Task<ActionResult<ItemReadDTO>> AddItem([FromBody] ItemCreateDTO item)
        {
            var result = await _itemService.AddItem(item);

            // Use FromServiceResult logic but return 201 Created on success
            if (result == null)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Downstream service returned null result.");
            }

            if (!result.Status || result.Data == null)
            {
                var message = string.IsNullOrWhiteSpace(result.Message)
                    ? "Failed to create item."
                    : result.Message;

                return BadRequest(message);
            }

            var created = result.Data;

            return CreatedAtAction(nameof(GetItemById), new { id = created.Id }, created);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ItemUpdateDTO item)
        {
            var result = await _itemService.UpdateItem(id, item);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Downstream service returned null result.");
            }

            if (!result.Status)
            {
                var message = string.IsNullOrWhiteSpace(result.Message)
                    ? "Failed to update item."
                    : result.Message;

                return BadRequest(message);
            }


            return NoContent(); // 204
        }



        [Authorize(Policy = "Everyone")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var result = await _itemService.DeleteItem(id);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Downstream service returned null result.");
            }

            if (!result.Status)
            {
                var message = string.IsNullOrWhiteSpace(result.Message)
                    ? "Failed to delete item."
                    : result.Message;

                return BadRequest(message);
            }


            return NoContent(); // 204
        }





    }
}