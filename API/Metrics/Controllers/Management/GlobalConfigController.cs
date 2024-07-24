using Business.Management.Controllers;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;



namespace Metrics.Controllers.Management
{
    [Route("[controller]")]
    [ApiController]
    public partial class GlobalConfigController : GlobalConfigBaseController
    {

        public GlobalConfigController(IGlobalConfig_PROVIDER globalSettings_Provider)
            : base(globalSettings_Provider)
        {
        }

    }
}
