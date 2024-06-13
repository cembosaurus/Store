using Business.Management.Appsettings.Models;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace API_Gateway.Controllers.Management
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class RemoteServiceController : ControllerBase
    {
        private IRemoteServices_Provider _remoteServicesInfoService;



        public RemoteServiceController(IRemoteServices_Provider remoteServicesInfoService)
        {
            _remoteServicesInfoService = remoteServicesInfoService;
        }





        [Authorize(Policy = "Everyone")]
        [HttpPut()]
        public ActionResult UpdateAllServicesURL([FromBody] IEnumerable<Service_Model_AS> servicesURLs)
        {
            var result = _remoteServicesInfoService.Update(servicesURLs);

            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
