using API_Gateway.Controllers;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Payment.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Payment.Controllers.Business
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : AppControllerBase
    {

        private readonly IServiceResultFactory _resultFact;

        public PaymentController(IServiceResultFactory resultFact)
        {
            _resultFact = resultFact;
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost]
        public async Task<object> MakePayment(OrderPaymentCreateDTO order)
        {
            // To Do: implement real service

            var result = _resultFact.Result(order, true, "Payment successful.");

            return result;  // ctr res
        }


    }
}
