using Business.Metrics.DTOs;
using Business.Metrics.Tools.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Metrics.Tools
{
    public class MetricsDataTool : IMetricsDataTool
    {





        public List<KeyValuePair<string, string[]>> HeadersToList(IHeaderDictionary headers)
        {
            var metricsData = headers
                .Where(rh => rh.Key.StartsWith("Metrics."))
                .Select(s => new KeyValuePair<string, string[]>(s.Key, s.Value.ToArray()))
                .ToList();

            return metricsData;
        }


        public string ListToData(List<KeyValuePair<string, string[]>> data)
        {


            return null;
        }

    }
}
