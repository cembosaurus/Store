using API_Gateway.Controllers;
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
    public class CartController : AppControllerBase
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
        public async Task<object> GetCarts([FromRoute] GetCarts_Q query)
        {
            var result = await _mediator.Send(query);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // customer
        [HttpGet]
        public async Task<object> GetCart([FromRoute] GetCart_Q query)
        {
            query.UserId = _principalId;

            var result = await _mediator.Send(query);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // management
        [HttpGet("{UserId}")]
        public async Task<object> GetUsersCart([FromRoute] GetCart_Q query)
        {
            var result = await _mediator.Send(query);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // management
        [HttpGet("exists")]
        public async Task<object> ExistsCartByCartId([FromBody] ExistsCartByCartId_Q query)
        {
            var result = await _mediator.Send(query);

            return result;  // ctr res
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]       // manager
        [HttpGet("items/all")]
        public async Task<object> GetAllCardItems([FromRoute] GetAllCardItems_Q query)
        {
            var result = await _mediator.Send(query);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // customer
        [HttpGet("items")]
        public async Task<object> GetCartItems([FromRoute] GetCartItems_Q query)
        {
            query.UserId = _principalId;

            var result = await _mediator.Send(query);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // manager
        [HttpGet("{UserId}/items")]
        public async Task<object> GetUsersCartItems([FromRoute] GetCartItems_Q query)
        {
            var result = await _mediator.Send(query);

            return result;  // ctr res
        }




        // POST:

        // Cart:


        [Authorize(Policy = "Everyone")]            // only customer
        [HttpPost]
        public async Task<object> CreateCart([FromRoute] CreateCart_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result;  // ctr res
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]            // customer
        [HttpPost("items")]
        public async Task<object> AddItemsToCart([FromBody] AddItemsToCart_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpPost("{UserId}/items")]
        public async Task<object> AddItemsToUsersCart([FromRoute] AddItemsToCart_C user, [FromBody] AddItemsToCart_C command)
        {
            command.UserId = user.UserId;

            var result = await _mediator.Send(command);

            return result;  // ctr res
        }




        // PUT:

        // Cart:

        [Authorize(Policy = "Everyone")]        // customer
        [HttpPut]
        public async Task<object> UpdateCart([FromBody] UpdateCart_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // management
        [HttpPut("{UserId}")]
        public async Task<object> UpdateUsersCart([FromRoute] UpdateCart_C user, [FromBody] UpdateCart_C command)
        {
            command.UserId = user.UserId;

            var result = await _mediator.Send(command);

            return result;  // ctr res
        }




        // DELETE:

        // Cart:


        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete]
        public async Task<object> DeleteCart([FromRoute] DeleteCart_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}")]
        public async Task<object> DeleteUsersCart([FromRoute] DeleteCart_C command)
        {
            var result = await _mediator.Send(command);

            return result;  // ctr res
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete("items")]
        public async Task<object> RemoveCartItems([FromBody] RemoveCartItems_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}/items")]
        public async Task<object> RemoveUsersCartItems([FromRoute] RemoveCartItems_C user, [FromBody] RemoveCartItems_C command)
        {
            command.UserId = user.UserId;

            var result = await _mediator.Send(command);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete("items/delete")]
        public async Task<object> DeleteCartItems([FromBody] DeleteCartItems_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}/items/delete")]
        public async Task<object> DeleteUsersCartItems([FromRoute] DeleteCartItems_C user, [FromBody] DeleteCartItems_C command)
        {
            command.UserId = user.UserId;

            var result = await _mediator.Send(command);

            return result;  // ctr res
        }



        // request initiated by API services (Scheduler Service), NOT by users:
        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpDelete("items/expired")]
        public async Task<object> RemoveExpiredItemsFromCart(DeleteExpiredCartItems_C command)
        {
            var result = await _mediator.Send(command);

            return result;  // ctr res
        }



    }
}
