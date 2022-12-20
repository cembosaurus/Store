using API_Gateway.HttpServices.Ordering.Interfaces;
using API_Gateway.Services.Ordering.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;

namespace API_Gateway.Services.Ordering
{
    public class OrderService : IOrderService
    {

        private readonly IHttpOrderService _httpOrderService;


        public OrderService(IHttpOrderService httpOrderService)
        {
            _httpOrderService = httpOrderService;
        }




        public async Task<IServiceResult<OrderReadDTO>> CompleteOrder(int userId)
        {
            return await _httpOrderService.CompleteOrder(userId);
        }



        public async Task<IServiceResult<OrderReadDTO>> CreateOrder(int userId, OrderCreateDTO orderCreateDTO)
        {
            return await _httpOrderService.CreateOrder(userId, orderCreateDTO);
        }



        public async Task<IServiceResult<OrderReadDTO>> DeleteOrder(int userId)
        {
            return await _httpOrderService.DeleteOrder(userId);
        }



        public async Task<IServiceResult<IEnumerable<OrderReadDTO>>> GetAllOrders()
        {
            return await _httpOrderService.GetAllOrders();
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByCartId(Guid cartId)
        {
            return await _httpOrderService.GetOrderByCartId(cartId);
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByOrderCode(string code)
        {
            return await _httpOrderService.GetOrderByOrderCode(code);
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByUserId(int userId)
        {
            return await _httpOrderService.GetOrderByUserId(userId);
        }



        public async Task<IServiceResult<OrderReadDTO>> UpdateOrder(int userId, OrderUpdateDTO orderUpdateDTO)
        {
            return await _httpOrderService.UpdateOrder(userId, orderUpdateDTO);
        }
    }
}
