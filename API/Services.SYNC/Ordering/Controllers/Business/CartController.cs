using Business.Filters.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.CQRS.Commands.Cart;
using Ordering.CQRS.Queries.Cart;
using System.Security.Claims;



namespace Ordering.Controllers.Business
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly int _principalId;
        private readonly IMediator _mediator;

        public CartController(IHttpContextAccessor accessor, IMediator mediator)
        {
            int.TryParse(accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out _principalId);
            _mediator = mediator;
        }





        // GET:

        // Cart:

        [Authorize(Policy = "Everyone")]            // management
        [HttpGet("all")]
        public async Task<IActionResult> GetCarts([FromRoute] GetCarts_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // customer
        [HttpGet]
        public async Task<IActionResult> GetCart([FromRoute] GetCart_Q query)
        {
            query.UserId = _principalId;

            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // management
        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetUsersCart([FromRoute] GetCart_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // management
        [HttpGet("exists")]
        public async Task<IActionResult> ExistsCartByCartId([FromBody] ExistsCartByCartId_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]       // manager
        [HttpGet("items/all")]
        public async Task<IActionResult> GetAllCardItems([FromRoute] GetAllCardItems_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // customer
        [HttpGet("items")]
        public async Task<IActionResult> GetCartItems([FromRoute] GetCartItems_Q query)
        {
            query.UserId = _principalId;

            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // manager
        [HttpGet("{UserId}/items")]
        public async Task<IActionResult> GetUsersCartItems([FromRoute] GetCartItems_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        // POST:

        // Cart:


        [Authorize(Policy = "Everyone")]            // only customer
        [HttpPost]
        public async Task<IActionResult> CreateCart([FromRoute] CreateCart_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]            // customer
        [HttpPost("items")]
        public async Task<IActionResult> AddItemsToCart([FromBody] AddItemsToCart_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpPost("{UserId}/items")]
        public async Task<IActionResult> AddItemsToUsersCart([FromRoute] AddItemsToCart_C user, [FromBody] AddItemsToCart_C command)
        {
            command.UserId = user.UserId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        // PUT:

        // Cart:

        [Authorize(Policy = "Everyone")]        // customer
        [HttpPut]
        public async Task<IActionResult> UpdateCart([FromBody] UpdateCart_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // management
        [HttpPut("{UserId}")]
        public async Task<IActionResult> UpdateUsersCart([FromRoute] UpdateCart_C user, [FromBody] UpdateCart_C command)
        {
            command.UserId = user.UserId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        // DELETE:

        // Cart:


        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete]
        public async Task<IActionResult> DeleteCart([FromRoute] DeleteCart_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}")]
        public async Task<IActionResult> DeleteUsersCart([FromRoute] DeleteCart_C command)
        {
            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete("items")]
        public async Task<IActionResult> RemoveCartItems([FromBody] RemoveCartItems_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}/items")]
        public async Task<IActionResult> RemoveUsersCartItems([FromRoute] RemoveCartItems_C user, [FromBody] RemoveCartItems_C command)
        {
            command.UserId = user.UserId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete("items/delete")]
        public async Task<IActionResult> DeleteCartItems([FromBody] DeleteCartItems_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}/items/delete")]
        public async Task<IActionResult> DeleteUsersCartItems([FromRoute] DeleteCartItems_C user, [FromBody] DeleteCartItems_C command)
        {
            command.UserId = user.UserId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        // request initiated by API services (Scheduler Service), NOT by users:
        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpDelete("items/expired")]
        public async Task<IActionResult> RemoveExpiredItemsFromCart(DeleteExpiredCartItems_C command)
        {
            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



    }
}
