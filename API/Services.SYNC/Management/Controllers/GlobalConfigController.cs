using Business.Filters.Identity;
using Business.Management.Appsettings.Interfaces;
using Microsoft.AspNetCore.Mvc;



namespace Management.Controllers
{

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
        [HttpGet()]
        public async Task<object> GetGlobalConfig()
        {
            var result = _appsettings_Provider.GetGlobalConfig();

            return result;  // ctr res
        }



        [ApiKeyAuth]
        [HttpGet("services")]
        public async Task<object> GetAllServiceModels()
        {
            var result = _appsettings_Provider.GetAllRemoteServicesModels();

            return result;  // ctr res
        }




    }
}
