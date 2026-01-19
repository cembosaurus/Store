using API_Gateway.Controllers;
using Business.Filters.Identity;
using Business.Scheduler.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Services.Interfaces;



namespace Scheduler.Controllers.Business
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SchedulerController : AppControllerBase
    {
        private readonly ICartItemsService _cartItemService;


        public SchedulerController(ICartItemsService cartItemService)
        {
            _cartItemService = cartItemService;
        }





        [Authorize(Policy = "Everyone")]
        [HttpGet("cartitem")]
        public async Task<object> GetAll()
        {
            var result = await _cartItemService.GetAllLocks();

            return result;  // ctr res
        }



        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpPost("cartitem/lock")]
        public async Task<object> LockCartItem(CartItemsLockCreateDTO cartItemsLock)
        {
            var result = await _cartItemService.CartItemsLock(cartItemsLock);

            return result;  // ctr res
        }



        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpDelete("cartitem/lock")]
        public async Task<object> UnLockCartItem(CartItemsLockDeleteDTO cartItemUnLock)
        {
            var result = await _cartItemService.CartItemsUnLock(cartItemUnLock);

            return result;  // ctr res
        }


    }
}
