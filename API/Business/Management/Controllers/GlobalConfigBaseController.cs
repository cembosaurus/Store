using Business.Filters.Identity;
using Business.Management.Appsettings.Models;
using Business.Management.Controllers.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;



namespace Business.Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public partial class GlobalConfigBaseController : ControllerBase, IHttpBroadcastController
    {
        private IGlobalConfig_PROVIDER _globalConfig_Provider;



        public GlobalConfigBaseController(IGlobalConfig_PROVIDER globalSettings_Provider)
        {
            _globalConfig_Provider = globalSettings_Provider;
        }






        [ApiKeyAuth]
        [HttpPut("remoteservices")]
        public ActionResult UpdateRemoteServiceModels([FromBody] IEnumerable<RemoteService_AS_MODEL> models)
        {
            var result = _globalConfig_Provider.UpdateRemoteServiceModels(models);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [ApiKeyAuth]
        [HttpPut()]
        public ActionResult Update([FromBody] Config_Global_AS_MODEL globalConfig)
        {
            var result = _globalConfig_Provider.Update(globalConfig);

            return result.Status ? Ok(result) : BadRequest(result);
        }

    }
}
