using Business.Filters.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Metrics.Controllers.Business
{

    [Route("[controller]")]
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
            var data = metricsData.ToList();


            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"------------------- {data} -------------------------- SENDING METRICS ");
            Console.ResetColor();


            return Ok();
        }



        //[ApiKeyAuth]
        //[HttpPost()]
        //public ActionResult TEST()
        //{

        //    Console.WriteLine("test");


        //    return Ok();
        //}


    }
}
