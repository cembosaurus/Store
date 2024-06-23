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

        private readonly IAppsettings_PROVIDER _appsettings_Provider;



        public RemoteServiceController(IAppsettings_PROVIDER appsettings_Provider)
        {
            _appsettings_Provider = appsettings_Provider;
        }





        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpGet("url/all")]
        public ActionResult GetAllServicesURL()
        {
            var result = _appsettings_Provider.GetAllRemoteServicesModels();

            return result.Status ? Ok(result) : BadRequest(result);
        }


    }
}
