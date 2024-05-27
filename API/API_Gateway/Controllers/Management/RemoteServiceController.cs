using Business.Management.Appsettings.Models;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace API_Gateway.Controllers.Management
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteServiceController : ControllerBase
    {
        private IRemoteServicesInfo_Provider _remoteServicesInfoService;



        public RemoteServiceController(IRemoteServicesInfo_Provider remoteServicesInfoService)
        {
            _remoteServicesInfoService = remoteServicesInfoService;
        }





        [Authorize(Policy = "Everyone")]
        [HttpPut()]
        public ActionResult UpdateAllServicesURL([FromBody] IEnumerable<Service_Model_AS> servicesURLs)
        {
            var result = _remoteServicesInfoService.UpdateServiceModels(servicesURLs);

            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
