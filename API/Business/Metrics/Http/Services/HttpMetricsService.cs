using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Metrics.Http.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;




namespace Business.Metrics.Http.Services
{
    public class HttpMetricsService : HttpBaseService, IHttpMetricsService
    {



        public HttpMetricsService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact, ConsoleWriter cm)
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact, cm)
        {
            _remoteServiceName = "MetricsService";
            _remoteServicePathName = "Collector";

        }




        //public async Task<IServiceResult<string>> Update(IEnumerable<KeyValuePair<string, StringValues>> metricsData)
        //{
        //    _method = HttpMethod.Post;
        //    _requestQuery = $"";
        //    _content = new StringContent(JsonConvert.SerializeObject(metricsData), _encoding, _mediaType);

        //    return await HTTP_Request_Handler<string>();
        //}
        public async Task<IServiceResult<string>> Update(IEnumerable<KeyValuePair<string, StringValues>> metricsData)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"";
            _content = new StringContent(JsonConvert.SerializeObject("******** TEST ********"), _encoding, _mediaType);



            var res = await HTTP_Request_Handler<string>();



            return res;
        }




    }
}
