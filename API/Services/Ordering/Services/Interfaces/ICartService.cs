using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;

namespace Ordering.Services.Interfaces
{
    public interface ICartService
    {
        Task<IServiceResult<CartReadDTO>> CreateCart(int userId);
        Task<IServiceResult<CartReadDTO>> DeleteCart(int id);
        Task<IServiceResult<bool>> ExistsCartByCartId(Guid cartId);
        Task<IServiceResult<IEnumerable<CartReadDTO>>> GetCards();
        Task<IServiceResult<CartReadDTO>> GetCartByUserId(int userId);
        Task<IServiceResult<CartReadDTO>> UpdateCart(int userId, CartUpdateDTO cartUpdateDTO);
    }
}
