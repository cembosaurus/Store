using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Payment.DTOs;

namespace Business.Payment.Http.Services.Interfaces
{
    public interface IHttpPaymentService
    {
        Task<IServiceResult<OrderReadDTO>> MakePayment(OrderPaymentCreateDTO order);
    }
}
