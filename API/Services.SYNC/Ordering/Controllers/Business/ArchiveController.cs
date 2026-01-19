using API_Gateway.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.CQRS.Commands.Archive;
using Ordering.CQRS.Queries.Archive;



namespace Ordering.Controllers.Business
{

    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ArchiveController : AppControllerBase
    {

        private readonly IMediator _mediator;

        public ArchiveController(IMediator mediator)
        {
            _mediator = mediator;
        }




        //[Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<object> GetAllOrders()
        {
            var result = await _mediator.Send(new GetAllOrders_Q());

            return result;  // ctr res
        }



        //[Authorize(Policy = "Everyone")]
        [HttpGet("user/{UserId}")]
        public async Task<object> GetOrderByUserId([FromRoute] GetOrderByUserId_Q query)
        {
            var result = await _mediator.Send(query);

            return result;  // ctr res
        }



        //[Authorize(Policy = "Everyone")]
        [HttpGet("cart")]
        public async Task<object> GetOrderByCartId([FromForm] GetOrderByCartId_Q query)
        {
            var result = await _mediator.Send(query);

            return result;  // ctr res
        }



        //[Authorize(Policy = "Everyone")]
        [HttpGet("code/{code}")]
        public async Task<object> GetOrderByOrderCode([FromRoute] GetOrderByOrderCode_Q query)
        {
            query.Code = System.Net.WebUtility.UrlDecode(query.Code);

            var result = await _mediator.Send(query);

            return result;  // ctr res
        }




        //[Authorize(Policy = "Everyone")]
        [HttpDelete("user/{UserId}")]
        public async Task<object> DeleteOrderPermanently([FromRoute] DeleteOrderPermanently_C command)
        {
            var result = await _mediator.Send(command);

            return result;  // ctr res
        }


    }
}
