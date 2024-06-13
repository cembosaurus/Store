using Business.Filters.Identity;
using Business.Scheduler.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Services.Interfaces;



namespace Scheduler.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly ICartItemsService _cartItemService;


        public SchedulerController(ICartItemsService cartItemService)
        {
            _cartItemService = cartItemService;
        }





        [Authorize(Policy = "Everyone")]
        [HttpGet("cartitem")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _cartItemService.GetAllLocks();

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpPost("cartitem/lock")]
        public async Task<IActionResult> LockCartItem(CartItemsLockCreateDTO cartItemsLock)
        {
            var result = await _cartItemService.CartItemsLock(cartItemsLock);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpDelete("cartitem/lock")]
        public async Task<IActionResult> UnLockCartItem(CartItemsLockDeleteDTO cartItemUnLock)
        {
            var result = await _cartItemService.CartItemsUnLock(cartItemUnLock);

            return result.Status ? Ok(result) : BadRequest(result);
        }


    }
}
