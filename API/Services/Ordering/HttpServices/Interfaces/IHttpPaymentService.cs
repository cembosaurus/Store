using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Payment.DTOs;

namespace Ordering.HttpServices.Interfaces
{
    public interface IHttpPaymentService
    {
        Task<IServiceResult<OrderReadDTO>> MakePayment(OrderPaymentCreateDTO order);
    }
}
