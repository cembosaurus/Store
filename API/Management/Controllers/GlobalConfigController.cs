using Business.Filters.Identity;
using Business.Management.Appsettings.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Management.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class GlobalConfigController : ControllerBase
    {

        private readonly IAppsettings_PROVIDER _appsettings_Provider;



        public GlobalConfigController(IAppsettings_PROVIDER appsettings_Provider)
        {
            _appsettings_Provider = appsettings_Provider;
        }





        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpGet()]
        public ActionResult GetGlobalConfig()
        {
            var result = _appsettings_Provider.GetGlobalConfig();

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpGet("services")]
        public ActionResult GetAllServiceModels()
        {
            var result = _appsettings_Provider.GetAllRemoteServicesModels();

            return result.Status ? Ok(result) : BadRequest(result);
        }


    }
}
