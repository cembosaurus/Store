﻿using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;



namespace Business.Http.Services
{
    public class HttpBaseService
    {

        private static IHttpContextAccessor _accessor;
        private readonly IHttpAppClient _httpAppClient;
        private readonly bool _isProdEnv;

        protected readonly IServiceResultFactory _resultFact;
        protected IGlobalConfig_PROVIDER _globalConfig_Provider;
        protected readonly IExId _exId;

        protected bool _useApiKey;
        protected RemoteService_AS_MODEL _service_model;
        protected HttpRequestMessage _requestMessage;
        protected string _remoteServiceName = "";
        protected string _remoteServicePathName = "";
        protected string _requestURL = "";
        protected string _requestQuery = "";
        protected HttpMethod _method;
        protected HttpContent _content;
        protected Dictionary<string, string> _requestHeaders = new Dictionary<string, string>();
        protected readonly string _mediaType = "application/json";
        protected readonly Encoding _encoding = Encoding.UTF8;
        protected static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";



        // any Http service (except 'HTTPManagementService'):
        public HttpBaseService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER globalConfig_Provider, IServiceResultFactory resultFact)
        {
            _accessor = accessor;
            _isProdEnv = env.IsProduction();
            _exId = exId;
            _httpAppClient = httpAppClient;
            _globalConfig_Provider = globalConfig_Provider;
            _resultFact = resultFact;
        }
        // 'HTTPManagementService': to prevent circulatory DI: HTTPManagementService <--> GlobalConfig_PROVIDER:
        public HttpBaseService(IWebHostEnvironment env, IHttpAppClient httpAppClient, IServiceResultFactory resultFact)
        {
            _isProdEnv = env.IsProduction();
            _httpAppClient = httpAppClient;
            _resultFact = resultFact;
        }





        protected async Task<IServiceResult<T>> HTTP_Request_Handler<T>()
        {
            // create HTTP request:

            var initResponse = await CreateRequest();

            if (!initResponse.Status)
                return _resultFact.Result(default(T), false, $"HTTP request for remote service '{_remoteServiceName}' was NOT created ! Reason: {initResponse.Message}");


            // send HTTP reequest:

            var sendResponse = await Send();


            // handle HTTP response:

            if (!sendResponse.IsSuccessStatusCode || sendResponse.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result(default(T), false, $"{(sendResponse.ReasonPhrase == "OK" ? "Fail" : sendResponse.ReasonPhrase)}: {sendResponse.RequestMessage?.Method}, {sendResponse.RequestMessage?.RequestUri}");

            var content = sendResponse.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<T>>(content);

            return result;
        }



        protected async virtual Task<IServiceResult<bool>> CreateRequest()
        {

            if (string.IsNullOrWhiteSpace(_remoteServiceName))
                return _resultFact.Result(false, false, $"Remote Service NOT found, service's name was NOT provided !");

            // Load service spec from local Global Config
            var modelResult = GetServiceModel_FromLocal();

            if (!modelResult.Status)
            {

                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine($"HttpBaseService: --> FAIL -- Load service model from Management Service for:   {_remoteServiceName} -- {modelResult.Message}   -----------------------------------");
                Console.ResetColor();



                // Local Global Config DB could be obsolete or incomplete,
                // Re-Load Global Config data from remote API source (Management Service):
                var GCResult = await _globalConfig_Provider.ReLoadGlobalConfig_FromRemote();

                if (!GCResult.Status)
                    return _resultFact.Result(false, false, $"Failed to re-load Global Config from Management service. URL for {_remoteServiceName} is not available !");

                // Make sure local Global Config was updated,
                // Load service model from local Global Config again:
                modelResult = GetServiceModel_FromLocal();

                if (!modelResult.Status)
                    return _resultFact.Result(false, false, $"Global Config with URL for '{_remoteServiceName}' was NOT found locally !");
            }

            _service_model = modelResult.Data ?? null!;


            // URL found in local Global Config,
            // build HTTP request URL:

            _requestURL = _service_model.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

            if (string.IsNullOrWhiteSpace(_requestURL))
                return _resultFact.Result(false, false, $"Request URL for Remote Service could not be constructed ! Missing URL in '{_remoteServiceName}' model !");


            // use API Key for relevant API services:

            _useApiKey = _remoteServicePathName == "GlobalConfig" || _remoteServicePathName == "Collector";

            if (_useApiKey)
            {
                var apiKeyResult = AddApiKeyToHeader();

                if (!apiKeyResult.Status)
                    return apiKeyResult;
            }


            // create HTTP request message:

            CreateHttpRequestMessage();


            return _resultFact.Result(true, true);
        }




        // VIRTUAL - unlike all HTTP Services, HttpManagementService (uses its own overridden Send()) doesn't need 503 ex handling because
        // if Management API service is not reached via HTTP request, then there is no logic in trying to update Management API service's URL from Management API service:
        protected async virtual Task<HttpResponseMessage> Send()
        {
            // FIRST attempt:
            try
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> HttpBaseService: --> calling:   {_remoteServiceName}   >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.ResetColor();

                return await _httpAppClient.SendAsync(_requestMessage);
            }
            catch (Exception ex) when (_exId.Http_503(ex))
            {

                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< HttpBaseService: 503 Exception - http client call:   {_remoteServiceName}  1 attempt failed   <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                Console.ResetColor();


                // Catch ex 503: if HTTP Request URL provided by global settings is not up-to-date or wrong, request will fail ! ...
                // Send the HTTP request to Management API service to update global settings by new data. ...
                // Create new request URL. ...
                // Try HTTP request again:


                var servicesModelsResult = await ReloadGloalConfig_FromRemote();

                if (!servicesModelsResult.Status)
                    throw new HttpRequestException($"HTTP 503: Request to remote service '{_remoteServiceName}' could NOT be completed due to incorrect URL. Attempt to get correct URL from 'Management' API Service FAILED ! \\n Message: {servicesModelsResult.Message}");


                _requestURL = _service_model.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

                if (string.IsNullOrWhiteSpace(_requestURL))
                    throw new HttpRequestException("Failed to get request URL from remote service model !");


                // SECOND attempt:

                CreateHttpRequestMessage();

                return await _httpAppClient.SendAsync(_requestMessage);
            }

        }



        protected void CreateHttpRequestMessage()
        {
            _requestMessage = new HttpRequestMessage { RequestUri = new Uri(_requestURL + (string.IsNullOrWhiteSpace(_requestQuery) ? "" : "/" + _requestQuery)) };
            _requestMessage.Method = _method;
            _requestMessage.Content = _content;
            _requestMessage.Options.Set(new HttpRequestOptionsKey<string>("RequestTo"), _remoteServiceName);

            // Headers:
            _requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            if (!string.IsNullOrWhiteSpace(_token) && !_useApiKey)
                _requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            if (!_requestHeaders.IsNullOrEmpty())
            {
                foreach (var h in _requestHeaders)
                {
                    _requestMessage.Headers.Add(h.Key, h.Value);
                }
            }
            _requestHeaders.Clear();
        }



        protected virtual IServiceResult<bool> AddApiKeyToHeader()
        {
            var apiKeyResult = _globalConfig_Provider.GetApiKey();

            if (apiKeyResult.Status)
                _requestHeaders.Add("x-api-key", apiKeyResult.Data ?? "");

            return _resultFact.Result(apiKeyResult.Status, apiKeyResult.Status, $"HTTP Request '{_remoteServiceName}/{_remoteServicePathName}': {apiKeyResult.Message}");
        }



        protected virtual IServiceResult<RemoteService_AS_MODEL> GetServiceModel_FromLocal()
        {
            return _globalConfig_Provider.GetRemoteServiceByName(_remoteServiceName);
        }


        protected virtual async Task<IServiceResult<Config_Global_AS_MODEL>> ReloadGloalConfig_FromRemote()
        {
            var GCResult = await _globalConfig_Provider.ReLoadGlobalConfig_FromRemote();

            if (!GCResult.Status)
                return _resultFact.Result<Config_Global_AS_MODEL>(null, false, $"Failed to fetch Global Config from Management service !");

            return GCResult;
        }
    }
}
