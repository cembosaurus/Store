using Business.Filters.Identity;
using Business.Http.Controllers.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Business.Http.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public partial class GlobalSettingsBaseController : ControllerBase, IHttpBroadcastController
    {
        private IGlobal_Settings_PROVIDER _globalSettings_Provider;



        public GlobalSettingsBaseController(IGlobal_Settings_PROVIDER globalSettings_Provider)
        {
            _globalSettings_Provider = globalSettings_Provider;
        }






        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpPut("remoteservices")]
        public ActionResult UpdateRemoteServiceModels([FromBody] IEnumerable<RemoteService_MODEL_AS> models)
        {
            var result = _globalSettings_Provider.UpdateRemoteServiceModels(models);

            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
