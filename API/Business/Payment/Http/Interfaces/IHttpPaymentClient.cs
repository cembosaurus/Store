using Business.Ordering.DTOs;
using Business.Payment.DTOs;

namespace Business.Payment.Http.Interfaces
{
    public interface IHttpPaymentClient
    {
        Task<HttpResponseMessage> MakePayment(OrderPaymentCreateDTO order);
    }
}
