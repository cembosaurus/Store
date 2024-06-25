using Business.Http.Controllers;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Identity.Controllers.Management
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public partial class GlobalConfigController : GlobalConfigBaseController
    {

        public GlobalConfigController(IGlobal_Settings_PROVIDER globalSettings_Provider)
            : base(globalSettings_Provider)
        {
        }

    }
}
