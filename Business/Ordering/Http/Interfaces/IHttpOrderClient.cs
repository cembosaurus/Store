using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;

namespace Business.Ordering.Http.Interfaces
{
    public interface IHttpOrderClient
    {
        Task<OrderReadDTO> AddOrder(OrderReadDTO orderDTO);
        Task DeleteOrder(int orderId);
        Task<IEnumerable<OrderReadDTO>> GetAllOrders();
        Task<OrderReadDTO> GetOrderById(int id);
        Task UpdateOrder(OrderReadDTO orderDTO);
    }
}
