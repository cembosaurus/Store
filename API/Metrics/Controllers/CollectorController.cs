using Business.Filters.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Metrics.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CollectorController : ControllerBase
    {



        public CollectorController()
        {

        }






        [ApiKeyAuth]
        [HttpPost()]
        public ActionResult AddMetricsData([FromBody] IEnumerable<KeyValuePair<string, StringValues>> metricsData)
        {

            // metrics data are sent in headers
            var v = metricsData.ToList();


            return Ok();
        }


    }
}
