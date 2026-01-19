using Business.Libraries.ServiceResult.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace API_Gateway.Controllers
{
    public abstract class AppControllerBase : ControllerBase
    {

        /// <summary>
        /// Maps IServiceResult<T> to proper HTTP responses.
        /// </summary>
        protected ActionResult<T> FromServiceResult<T>(IServiceResult<T> result)
        {

            if (result == null)
            {
                // Downstream service failed in some unexpected way.
                return StatusCode(StatusCodes.Status502BadGateway,
                    "Downstream service returned null result.");
            }


            if (!result.Status)
            {
                // Business/validation failure in downstream service.
                var message = string.IsNullOrWhiteSpace(result.Message)
                    ? "Downstream service reported failure."
                    : result.Message;

                // You could decide to sometimes use 500 instead of 400 here.
                return BadRequest(message);
            }


            if (result.Data == null)
            {
                // For GET-like operations, treat this as NotFound.
                return NotFound();
            }


            return Ok(result.Data);
        }

    }
}
