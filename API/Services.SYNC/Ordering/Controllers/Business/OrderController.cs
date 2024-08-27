using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.CQRS.Commands.Order;
using Ordering.CQRS.Queries.Order;
using System.Security.Claims;



namespace Ordering.Controllers.Business
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly int _principalId;
        private readonly IMediator _mediator;

        public OrderController(IHttpContextAccessor accessor, IMediator mediator)
        {
            int.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out _principalId);
            _mediator = mediator;
        }




        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders([FromRoute] GetAllOrders_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Customer
        [HttpGet]
        public async Task<IActionResult> GetOrderByUserId([FromRoute] GetOrderByUserId_Q query)
        {
            query.UserId = _principalId;

            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("user/{UserId}")]
        public async Task<IActionResult> GetUsersOrderByUserId([FromRoute] GetOrderByUserId_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("cart")]
        public async Task<IActionResult> GetUsersOrderByCartId([FromBody] GetOrderByCartId_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("ordercode")]
        public async Task<IActionResult> GetOrderByOrderCode([FromBody] GetOrderByOrderCode_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }





        [Authorize(Policy = "Everyone")]            // Customer
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpPost("{UserId}")]
        public async Task<IActionResult> CreateUsersOrder([FromRoute] CreateOrder_C user, [FromBody] CreateOrder_C command)
        {
            command.UserId = user.UserId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        [Authorize(Policy = "Everyone")]        // Customer
        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrder_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        [Authorize(Policy = "Everyone")]        // Management
        [HttpPut("{UserId}")]
        public async Task<IActionResult> UpdateUsersOrder([FromRoute] UpdateOrder_C user, [FromBody] UpdateOrder_C command)
        {
            command.UserId = user.UserId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        [Authorize(Policy = "Everyone")]            // Customer
        [HttpPut("complete")]
        public async Task<IActionResult> CompleteOrder([FromRoute] CompleteOrder_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpPut("{UserId}/complete")]
        public async Task<IActionResult> CompleteUsersOrder([FromRoute] CompleteOrder_C command)
        {
            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Customer
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder([FromRoute] DeleteOrder_C command)
        {
            command.UserId = _principalId;

            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpDelete("{UserId}")]
        public async Task<IActionResult> DeleteUsersOrder([FromRoute] DeleteOrder_C command)
        {
            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }


    }
}
