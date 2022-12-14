using Business.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Ordering.Models;

namespace Ordering.Data.Repositories.Interfaces
{
    public interface IArchiveRepository : IBaseRepository
    {
        Task DeleteCartsPermanently(IEnumerable<Cart> carts);
        Task DeleteOrdersPermanently(IEnumerable<Order> orders);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Cart> GetCartById(Guid cartId);
        Task<Cart> GetCartByUserId(int userId);
        Task<Guid> GetCartIdByUserId(int userId);
        Task<IEnumerable<Cart>> GetCartsByUserId(int userId);
        Task<Cart> GetCartWithOrderByUserId(int userId);
        Task<Order> GetOrderByCartId(Guid cardId);
        Task<Order> GetOrderByOrderCode(string code);
        Task<IEnumerable<Order>> GetOrdersByUserId(int userId);
    }
}
