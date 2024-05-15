using Business.Management.Appsettings.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Management.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteServiceController : ControllerBase
    {

        private readonly IAppsettingsService _appsettings;



        public RemoteServiceController(IAppsettingsService appsettings)
        {
            _appsettings = appsettings;
        }




        [Authorize(Policy = "Everyone")]
        [HttpGet("url/all")]
        public ActionResult GetAllServicesURL()
        {
            var result = _appsettings.GetAllRemoteServicesURL();

            return result.Status ? Ok(result) : BadRequest(result);
        }



        //[HttpGet("{id}", Name = "GetServiceById")]
        //public async Task<ActionResult> GetServiceInfoById([FromRoute]Guid id)
        //{
        //    var result = await _remoteServices.GetRemoteServiceInfo(id);

        //    return result.Status ? Ok(result) : BadRequest(result);
        //}

    }
}
