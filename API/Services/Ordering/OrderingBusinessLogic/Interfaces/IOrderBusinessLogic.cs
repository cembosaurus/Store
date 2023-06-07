using Business.Libraries.ServiceResult.Interfaces;
using Services.Ordering.Models;

namespace Ordering.OrderingBusinessLogic.Interfaces
{
    public interface IOrderBusinessLogic
    {
        IServiceResult<string> CreateOrderCode(int cartId);
        IServiceResult<string> DecodeOrderCode(string orderId);
    }
}
