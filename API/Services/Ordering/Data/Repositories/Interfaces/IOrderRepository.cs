using Business.Data.Repositories.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Ordering.Models;

namespace Ordering.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository
    {
        Task<EntityEntry> CreateOrder(Order order);
        Task<EntityEntry> CreateOrderDetails(OrderDetails orderDetails);
        Task<EntityEntry> DeleteOrder(Order order);
        Task<EntityEntry> DeleteOrderDetails(OrderDetails orderDetails);
        Task<bool> ExistsOrderByCartId(Guid cartId);
        Task<bool> ExistsOrderDetailsByCartId(Guid cartId);
        Task<IEnumerable<OrderDetails>> GetAllOrderDetails();
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderByCartId(Guid cardId);
        Task<Order> GetOrderByOrderCode(string code);
        Task<Order> GetOrderByUserId(int userId);
        Task<OrderDetails> GetOrderDetailsByCartId(Guid cartId);
        Task<Order> GetOrderWithDetailsByUserId(int userId);

        //Task<bool> ExistsOrderDetailsByOrderId(string id);
        //Task<OrderDetails> GetOrderDetailByOrderId(string id);
    }
}
