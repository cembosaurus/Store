using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services;
using Business.Identity.DTOs;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Business.Identity.Http.Services
{
    public class HttpIdentityService : HttpBaseService, IHttpIdentityService
    {

        public HttpIdentityService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact, ConsoleWriter cm)
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact, cm)
        {
            _remoteServiceName = "IdentityService";
            _remoteServicePathName = "Identity";
        }





        public async Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"register";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            return await HTTP_Request_Handler<UserAuthDTO>();
        }



        public async Task<IServiceResult<string>> Login_UserPassword(UserToLoginDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"login";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            return await HTTP_Request_Handler<string>();
        }



        // NOT USED. ApiKey is used to directly authenticate api service. No JWT necessary:
        public async Task<IServiceResult<string>> Login_ApiKey(string apiKey)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"service/authenticate";
            _requestHeaders.Add("x-api-key", apiKey);

            return await HTTP_Request_Handler<string>();
        }

    }
}
