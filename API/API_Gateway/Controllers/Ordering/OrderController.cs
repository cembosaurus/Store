using API_Gateway.Services.Ordering.Interfaces;
using Business.Ordering.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Gateway.Controllers.Ordering
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly int _principalId;
        private readonly IOrderService _orderService;

        public OrderController(IHttpContextAccessor accessor, IOrderService orderService)
        {
            int.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out _principalId);
            _orderService = orderService;
        }





        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrders();

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Customer
        [HttpGet]
        public async Task<IActionResult> GetOrderByUserId()
        {
            var result = await _orderService.GetOrderByUserId(_principalId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUsersOrderByUserId(int userId)
        {
            var result = await _orderService.GetOrderByUserId(userId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("cart/{cartId}")]
        public async Task<IActionResult> GetOrderByCartId(Guid cartId)
        {
            var result = await _orderService.GetOrderByCartId(cartId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("ordercode")]
        public async Task<IActionResult> GetOrderByOrderCode(string orderCode)
        {
            var result = await _orderService.GetOrderByOrderCode(orderCode);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        [Authorize(Policy = "Everyone")]            // Customer
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            var result = await _orderService.CreateOrder(_principalId, orderCreateDTO);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateUsersOrder([FromRoute] int userId, [FromBody] OrderCreateDTO orderCreateDTO)
        {
            var result = await _orderService.CreateOrder(userId, orderCreateDTO);   

            return result.Status ? Ok(result) : BadRequest(result);
        }




        [Authorize(Policy = "Everyone")]        // Customer
        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            var result = await _orderService.UpdateOrder(_principalId, orderUpdateDTO);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        [Authorize(Policy = "Everyone")]        // Management
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUsersOrder([FromRoute] int userId, [FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            var result = await _orderService.UpdateOrder(userId, orderUpdateDTO);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        [Authorize(Policy = "Everyone")]            // Customer
        [HttpPut("complete")]
        public async Task<IActionResult> CompleteOrder()
        {
            var result = await _orderService.CompleteOrder(_principalId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpPut("{userId}/complete")]
        public async Task<IActionResult> CompleteUsersOrder(int userId)
        {
            var result = await _orderService.CompleteOrder(userId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Customer
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder()
        {
            var result = await _orderService.DeleteOrder(_principalId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUsersOrder(int userId)
        {
            var result = await _orderService.DeleteOrder(userId);

            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
