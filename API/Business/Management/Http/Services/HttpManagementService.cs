﻿using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.DTOs;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Http.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;



namespace Business.Management.Http.Services
{
    // sends HTTP messages to Management API service.
    // URL routes: source Appsettings

    public class HttpManagementService : HttpBaseService, IHttpManagementService
    {

        private readonly IExId _exId;
        private readonly IHttpAppClient _httpAppClient;
        private IAppsettings_PROVIDER _appsettings_Provider;


        public HttpManagementService(IWebHostEnvironment env, IExId exId, IAppsettings_PROVIDER appsettings_Provider, IHttpAppClient httpAppClient, IServiceResultFactory resultFact)
            : base(env, httpAppClient, resultFact)
        {
            _remoteServiceName = "ManagementService";
            _remoteServicePathName = "GlobalConfig";
            _exId = exId;
            _httpAppClient = httpAppClient;
            _appsettings_Provider = appsettings_Provider;
        }





        protected async override Task<HttpResponseMessage> Send()
        {
            //try
            //{
            //    Console.BackgroundColor = ConsoleColor.Blue;
            //    Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> HttpManagementService: --> calling:   {_remoteServiceName}   >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            //    Console.ResetColor();


                return await _httpAppClient.SendAsync(_requestMessage);
            //}
            //catch (Exception ex) when (_exId.Http_503(ex))
            //{
            //    Console.BackgroundColor = ConsoleColor.DarkBlue;
            //    Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< HttpManagementService: 503 Exception - http client call:   {_remoteServiceName}  UNHANDLED   <<<<<<<<<<<<<<<<<<< RE-THROW <<<<<<<<<<<<<<<<<<");
            //    Console.ResetColor();

            //    throw;
            //}

        }


        protected override IServiceResult<RemoteService_AS_MODEL> GetServiceModel_FromLocal()
        {
            return _appsettings_Provider.GetRemoteServiceModel(_remoteServiceName);
        }


        protected override IServiceResult<bool> AddApiKeyToHeader()
        {
            var apiKeyResult = _appsettings_Provider.GetApiKey();

            if (apiKeyResult.Status)
                _requestHeaders.Add("x-api-key", apiKeyResult.Data ?? "");

            return _resultFact.Result(apiKeyResult.Status, apiKeyResult.Status, $"HTTP Request '{_remoteServiceName}/{_remoteServicePathName}': {apiKeyResult.Message}");
        }






        public async Task<IServiceResult<Config_Global_AS_DTO>> GetGlobalConfig()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"";

            try
            {
                return await HTTP_Request_Handler<Config_Global_AS_DTO>();

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS HTTP MANAGEMENT SERVICE get global config.... FAIL..... SSSSSSSSSSSSSSSSSSSSSSSSSSSS");
                return _resultFact.Result<Config_Global_AS_DTO>(null, false, ex.Message);
            }
        }


        public async Task<IServiceResult<IEnumerable<RemoteService_AS_DTO>>> GetAllRemoteServices()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{"services"}";

            return await HTTP_Request_Handler<IEnumerable<RemoteService_AS_DTO>>();
        }




        public async Task<IServiceResult<RemoteService_AS_DTO>> AddRemoteService(RemoteService_AS_DTO service)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"{"services/add"}";
            _content = new StringContent(JsonConvert.SerializeObject(service), _encoding, _mediaType);

            return await HTTP_Request_Handler<RemoteService_AS_DTO>();
        }

    }
}
