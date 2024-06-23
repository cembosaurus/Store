using Business.Http.Controllers;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace API_Gateway.Controllers.Management
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public partial class GlobalSettingsController : GlobalSettingsBaseController
    {

        public GlobalSettingsController(IGlobal_Settings_PROVIDER globalSettings_Provider)
            : base(globalSettings_Provider)
        {
        }

    }
}
