using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;

namespace API_Gateway.Services.Business.Ordering.Interfaces
{
    public interface IOrderService
    {
        Task<IServiceResult<OrderReadDTO>> CompleteOrder(int userId);
        Task<IServiceResult<OrderReadDTO>> CreateOrder(int userId, OrderCreateDTO orderCreateDTO);
        Task<IServiceResult<OrderReadDTO>> DeleteOrder(int userId);
        Task<IServiceResult<IEnumerable<OrderReadDTO>>> GetAllOrders();
        Task<IServiceResult<OrderReadDTO>> GetOrderByCartId(Guid cartId);
        Task<IServiceResult<OrderReadDTO>> GetOrderByOrderCode(string code);
        Task<IServiceResult<OrderReadDTO>> GetOrderByUserId(int userId);
        Task<IServiceResult<OrderReadDTO>> UpdateOrder(int userId, OrderUpdateDTO orderUpdateDTO);
    }
}
