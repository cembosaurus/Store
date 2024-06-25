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
    public partial class GlobalConfigBaseController : ControllerBase, IHttpBroadcastController
    {
        private IGlobal_Settings_PROVIDER _globalSettings_Provider;



        public GlobalConfigBaseController(IGlobal_Settings_PROVIDER globalSettings_Provider)
        {
            _globalSettings_Provider = globalSettings_Provider;
        }






        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpPut("remoteservices")]
        public ActionResult UpdateRemoteServiceModels([FromBody] IEnumerable<RemoteService_AS_MODEL> models)
        {
            var result = _globalSettings_Provider.UpdateRemoteServiceModels(models);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpPut()]
        public ActionResult Update([FromBody] Config_Global_AS_MODEL globalConfig)
        {
            var result = _globalSettings_Provider.Update(globalConfig);

            return result.Status ? Ok(result) : BadRequest(result);
        }

    }
}
