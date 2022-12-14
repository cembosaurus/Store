using Business.Identity.Http.Clients.Interfaces;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.JWT.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Business.Identity.Http.Services
{
    public class HttpApiKeyAuthService : IHttpApiKeyAuthService
    {

        private readonly IHttpIdentityClient _httpIdentityClient;
        private readonly IServiceResultFactory _resultFact;
        private readonly string? _apiKey;
        private readonly IHttpContextAccessor _accessor;
        private readonly IJWTTokenStore _jwtTokenStore;

        public HttpApiKeyAuthService(IConfiguration config, IServiceResultFactory resultFact, IHttpIdentityClient httpIdentityClient, IHttpContextAccessor accessor, IJWTTokenStore jwtTokenStore)
        {
            _httpIdentityClient = httpIdentityClient;
            _resultFact = resultFact;
            _apiKey = config.GetSection("ApiKey").Value ?? "";
            _accessor = accessor;
            _ = _accessor.HttpContext ?? new DefaultHttpContext();
            _jwtTokenStore = jwtTokenStore;
        }





        public async Task<IServiceResult<string>> AuthenticateService()
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return _resultFact.Result("", true, "FAILED to retrieve ApiKey from settings !");

            var response = await _httpIdentityClient.AuthenticateService(_apiKey);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result("", false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var tokenResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<string>>(content);


            if (tokenResult == null)
            {
                Console.WriteLine(
                    $"\r\n********************************* WARNING ******************************" +
                    $"\r\n*                                                                      *" +
                    $"\r\n*                                                                      *" +
                    $"\r\n*            FAILED to get response from Identity Service !            *" +
                    $"\r\n*                                                                      *" +
                    $"\r\n*                                                                      *" +
                    $"\r\n************************************************************************" +
                    tokenResult?.Message
                    );

                return _resultFact.Result("", false, "FAILED to get response from Identity Service !");
            }
            else if (!tokenResult.Status)
            {
                Console.WriteLine(
                    $"\r\n********************************* WARNING ******************************" +
                    $"\r\n*                                                                      *" +
                    $"\r\n*                                                                      *" +
                    $"\r\n*                   FAILED to authenticate Service !                   *" +
                    $"\r\n*                                                                      *" +
                    $"\r\n*                                                                      *" +
                    $"\r\n************************************************************************" +
                    tokenResult?.Message
                    );

                return _resultFact.Result("", false, tokenResult.Message);
            }

            Console.WriteLine(
                $"\r\n***************************** SUCCESS **********************************" +
                $"\r\n*                                                                      *" +
                $"\r\n*                                                                      *" +
                $"\r\n*        Authentication TOKEN received from Identity Service.          *" +
                $"\r\n*                                                                      *" +
                $"\r\n*                                                                      *" +
                $"\r\n************************************************************************" +
                tokenResult?.Message
            );

            _accessor.HttpContext?.Request.Headers.Add("Authorization", new AuthenticationHeaderValue("Bearer", tokenResult.Data).ToString());

            _jwtTokenStore.Token = tokenResult.Data;

            return tokenResult;
        }






    }
}
