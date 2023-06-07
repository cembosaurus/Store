using Business.Libraries.ServiceResult.Interfaces;
using Business.Payment.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IServiceResultFactory _resultFact;

        public PaymentController(IServiceResultFactory resultFact)
        {
            _resultFact = resultFact;
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost]
        public async Task<ActionResult> MakePayment(OrderPaymentCreateDTO order)
        {
            // To Do: implement real service

            var result = _resultFact.Result(order, true, "Payment successful.");

            return result.Status ? Ok(result) : BadRequest(result);
        }


    }
}
