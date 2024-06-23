using Business.Filters.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Business.Http.Controllers.Interfaces
{
    public interface ITestController
    {
        [ApiKeyAuth]
        [AllowAnonymous]
        [HttpPut("abc/def")]
        ActionResult Test();
    }
}
