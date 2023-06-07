using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Models;
using Scheduler.Services.Interfaces;

namespace Scheduler.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ICartItemsService _cartItemService;
        private readonly IServiceResultFactory _resultFact;

        public SchedulerController(ICartItemsService cartItemService, IServiceResultFactory resultFact, IIdentityService identityService)
        {
            _identityService = identityService;
            _cartItemService = cartItemService;
            _resultFact = resultFact;
        }





        [Authorize(Policy = "Everyone")]
        [HttpGet("cartitem")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _cartItemService.GetAllLocks();

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost("cartitem/lock")]
        public async Task<IActionResult> LockCartItem(CartItemsLockCreateDTO cartItemsLock)
        {
            var result = await _cartItemService.CartItemsLock(cartItemsLock);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpDelete("cartitem/lock")]
        public async Task<IActionResult> UnLockCartItem(CartItemsLockDeleteDTO cartItemUnLock)
        {
            var result = await _cartItemService.CartItemsUnLock(cartItemUnLock);

            return result.Status ? Ok(result) : BadRequest(result);
        }


    }
}
