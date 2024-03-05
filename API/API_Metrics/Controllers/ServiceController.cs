using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Metrics.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {





        [HttpGet("{id}", Name = "GetServiceById")]
        public async Task<ActionResult> GetServiceById([FromRoute]Guid id)
        {
            var result = id;// await _itemService.GetItemById(id);

            return Ok(result);// result.Status ? Ok(result) : BadRequest(result);
        }

    }
}
