using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;

namespace Ordering.Services.Interfaces
{
    public interface IArchiveService
    {
        Task<IServiceResult<IEnumerable<Guid>>> DeleteOrdersPermanently(int userId);
        Task<IServiceResult<IEnumerable<OrderReadDTO>>> GetAllOrders();
        Task<IServiceResult<OrderReadDTO>> GetOrderByCartId(Guid cartId);
        Task<IServiceResult<OrderReadDTO>> GetOrderByOrderCode(string code);
        Task<IServiceResult<OrderReadDTO>> GetOrderByUserId(int userId);
    }
}
