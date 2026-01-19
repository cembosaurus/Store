using API_Gateway.Services.Business.Ordering.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace API_Gateway.Controllers.Business.Ordering
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class OrderController : AppControllerBase
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
        public async Task<object> GetAllOrders()//;./  returns data instead actionresult
        {
            var result = await _orderService.GetAllOrders();

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // Customer
        [HttpGet]
        public async Task<object> GetOrderByUserId()
        {
            var result = await _orderService.GetOrderByUserId(_principalId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("user/{userId}")]
        public async Task<object> GetUsersOrderByUserId(int userId)
        {
            var result = await _orderService.GetOrderByUserId(userId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("cart/{cartId}")]
        public async Task<object> GetOrderByCartId(Guid cartId)
        {
            var result = await _orderService.GetOrderByCartId(cartId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpGet("ordercode")]
        public async Task<object> GetOrderByOrderCode(string orderCode)
        {
            var result = await _orderService.GetOrderByOrderCode(orderCode);

            return result;  // ctr res
        }




        [Authorize(Policy = "Everyone")]            // Customer
        [HttpPost]
        public async Task<object> CreateOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            var result = await _orderService.CreateOrder(_principalId, orderCreateDTO);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpPost("{userId}")]
        public async Task<object> CreateUsersOrder([FromRoute] int userId, [FromBody] OrderCreateDTO orderCreateDTO)
        {
            var result = await _orderService.CreateOrder(userId, orderCreateDTO);

            return result;  // ctr res
        }




        [Authorize(Policy = "Everyone")]        // Customer
        [HttpPut]
        public async Task<object> UpdateOrder([FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            var result = await _orderService.UpdateOrder(_principalId, orderUpdateDTO);

            return result;  // ctr res
        }




        [Authorize(Policy = "Everyone")]        // Management
        [HttpPut("{userId}")]
        public async Task<object> UpdateUsersOrder([FromRoute] int userId, [FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            var result = await _orderService.UpdateOrder(userId, orderUpdateDTO);

            return result;  // ctr res
        }




        [Authorize(Policy = "Everyone")]            // Customer
        [HttpPut("complete")]
        public async Task<object> CompleteOrder()
        {
            var result = await _orderService.CompleteOrder(_principalId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpPut("{userId}/complete")]
        public async Task<object> CompleteUsersOrder(int userId)
        {
            var result = await _orderService.CompleteOrder(userId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // Customer
        [HttpDelete]
        public async Task<object> DeleteOrder()
        {
            var result = await _orderService.DeleteOrder(_principalId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // Management
        [HttpDelete("user/{userId}")]
        public async Task<object> DeleteUsersOrder(int userId)
        {
            var result = await _orderService.DeleteOrder(userId);

            return result;  // ctr res
        }
    }
}
