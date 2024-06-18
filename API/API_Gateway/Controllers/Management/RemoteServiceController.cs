﻿using Business.Management.Appsettings.Models;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace API_Gateway.Controllers.Management
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class RemoteServiceController : ControllerBase
    {
        private IGlobal_Settings_PROVIDER _remoteServicesInfoService;



        public RemoteServiceController(IGlobal_Settings_PROVIDER remoteServicesInfoService)
        {
            _remoteServicesInfoService = remoteServicesInfoService;
        }





        [Authorize(Policy = "Everyone")]
        [HttpPut()]
        public ActionResult UpdateAllServicesURL([FromBody] IEnumerable<RemoteService_MODEL_AS> servicesURLs)
        {
            var result = _remoteServicesInfoService.UpdateRemoteServices(servicesURLs);

            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
