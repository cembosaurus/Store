using Business.Filters.Identity;
using Business.Management.Appsettings.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Management.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class RemoteServiceController : ControllerBase
    {

        private readonly IAppsettings_PROVIDER _appsettingsService;



        public RemoteServiceController(IAppsettings_PROVIDER appsettingsService)
        {
            _appsettingsService = appsettingsService;
        }




        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpGet("url/all")]
        public ActionResult GetAllServicesURL()
        {
            var result = _appsettingsService.GetAllRemoteServicesModels();

            return result.Status ? Ok(result) : BadRequest(result);
        }


        //[Authorize(Policy = "Everyone")]
        //[HttpGet("{id}", Name = "GetServiceById")]
        //public async Task<ActionResult> GetServiceInfoById([FromRoute]Guid id)
        //{
        //    var result = await _remoteServices.GetRemoteServiceInfo(id);

        //    return result.Status ? Ok(result) : BadRequest(result);
        //}

    }
}
