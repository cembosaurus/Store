using Business.Filters.Identity;
using Business.Management.Appsettings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Business.Management.Controllers.Interfaces
{
    public interface IHttpBroadcastController
    {
        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpPut("remoteservices/models")]
        ActionResult UpdateRemoteServiceModels([FromBody] IEnumerable<RemoteService_AS_MODEL> models);
    }
}
