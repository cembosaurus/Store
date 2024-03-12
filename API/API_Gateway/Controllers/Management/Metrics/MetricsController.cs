using API_Gateway.Services.Management.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace API_Gateway.Controllers.Management.Metrics
{




    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : ControllerBase
    {


        private readonly IRemoteServices _remoteServices;



        public MetricsController(IRemoteServices remoteServices)
        {
            _remoteServices = remoteServices;
        }




        [Authorize(Policy = "Everyone")]
        [HttpGet("all")]
        public async Task<ActionResult> GetAllServices()
        {
            var result = _remoteServices.GetAllRemoteServicesInfo();

            return result.Status ? Ok(result) : BadRequest(result);
        }


    }
}
