﻿using Business.Filters.Identity;
using Business.Metrics.DTOs;
using Metrics.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;



namespace Metrics.Controllers.Business
{

    [Route("[controller]")]
    [ApiController]
    public class CollectorController : ControllerBase
    {
        private readonly ICollectorService _collectorService;

        public CollectorController(ICollectorService collectorService)
        {
            _collectorService = collectorService;
        }






        [ApiKeyAuth]
        [HttpPost()]
        public ActionResult AddMetricsData(MetricsCreateDTO metricsData)
        {
            var addRecordResult = _collectorService.AddHttpTransactionRecord(metricsData);



            var data = metricsData.Data.ToList();


            //************************************** test *******************************************************************
            var t = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var v = DateTime.ParseExact(t, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Console.WriteLine(t);
            Console.WriteLine(v);
            Console.WriteLine($"{v.Year} - {v.Month} - {v.Day} - {v.Hour} - {v.Minute} - {v.Second} - {v.Millisecond}");

            var t1 = "2024-10-30 23:17:19.334";
            var t2 = "2024-10-30 23:18:21.679";
            var v1 = DateTime.ParseExact(t1, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var v2 = DateTime.ParseExact(t2, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            TimeSpan span = v2 - v1;
            int ms = (int)span.TotalMilliseconds;
            Console.WriteLine($"xxxxxxxxxxxxxxxxxxxxxxxx {ms} xxxxx {span.Minutes} - {span.Seconds} - {span.Milliseconds}");
            //*****************************************************************************************************************



            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"------------------- {data} -------------------------- GETTING METRICS ");
            Console.ResetColor();


            return Ok();
        }



    }
}
