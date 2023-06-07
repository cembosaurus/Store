using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.CQRS.Commands.Archive;
using Ordering.CQRS.Queries.Archive;

namespace Services.Ordering.Controllers
{

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ArchiveController(IMediator mediator)
        {
            _mediator = mediator;
        }




        //[Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _mediator.Send(new GetAllOrders_Q());

            return result.Status ? Ok(result) : BadRequest(result);
        }



        //[Authorize(Policy = "Everyone")]
        [HttpGet("user/{UserId}")]
        public async Task<IActionResult> GetOrderByUserId([FromRoute] GetOrderByUserId_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        //[Authorize(Policy = "Everyone")]
        [HttpGet("cart")]
        public async Task<IActionResult> GetOrderByCartId([FromForm] GetOrderByCartId_Q query)
        {
            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        //[Authorize(Policy = "Everyone")]
        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetOrderByOrderCode([FromRoute] GetOrderByOrderCode_Q query)
        {
            query.Code = System.Net.WebUtility.UrlDecode(query.Code);

            var result = await _mediator.Send(query);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        //[Authorize(Policy = "Everyone")]
        [HttpDelete("user/{UserId}")]
        public async Task<IActionResult> DeleteOrderPermanently([FromRoute] DeleteOrderPermanently_C command)
        {
            var result = await _mediator.Send(command);

            return result.Status ? Ok(result) : BadRequest(result);
        }


    }
}
