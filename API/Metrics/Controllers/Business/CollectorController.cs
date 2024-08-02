using Business.Filters.Identity;
using Business.Libraries.ServiceResult;
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






        //[ApiKeyAuth]
        //[HttpPost()]
        //public ActionResult AddMetricsData([FromBody] IEnumerable<KeyValuePair<string, StringValues>> metricsData)
        //{

        //    var data = metricsData.ToList();


        //    Console.BackgroundColor = ConsoleColor.Red;
        //    Console.WriteLine($"------------------- {data} -------------------------- SENDING METRICS ");
        //    Console.ResetColor();


        //    return Ok();
        //}
        [ApiKeyAuth]
        [HttpPost()]
        public ActionResult AddMetricsData([FromBody] string metricsData)
        {

            var data = metricsData.ToList();


            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"------------------- {metricsData} -------------------------- GETTING METRICS FROM API SERVICE ");
            Console.ResetColor();


            var a = new ServiceResultFactory();
            var result = a.Result<string>("test", true, "hovno");


            return Ok(result);
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
