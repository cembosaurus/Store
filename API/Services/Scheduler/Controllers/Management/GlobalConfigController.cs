﻿using Business.Http.Controllers;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Scheduler.Controllers.Management
{
    [Authorize]
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
