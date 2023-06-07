using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;

namespace Identity.HttpServices.Interfaces
{
    public interface IHttpCartService
    {
        Task<IServiceResult<CartReadDTO>> CreateCart(int userId);
    }
}
