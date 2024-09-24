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






        [ApiKeyAuth]
        [HttpPost()]
        public ActionResult AddMetricsData([FromBody] IEnumerable<KeyValuePair<string, string[]>> metricsData)
        {

            var data = metricsData.ToList();

            //var a = DateTime.ParseExact("2024-09-24 03:53:16.100", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //var b = DateTime.ParseExact("2024-09-24 04:55:19.223", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //var v = b - a;
            //var c = $"{v.Hours}, {v.Minutes}, {v.Seconds}, {v.Milliseconds}";



            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"------------------- {data} -------------------------- GETTING METRICS ");
            Console.ResetColor();


            return Ok();
        }



    }
}
