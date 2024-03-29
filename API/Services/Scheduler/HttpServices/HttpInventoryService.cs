﻿using Business.Inventory.Http.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Scheduler.HttpServices.Interfaces;

namespace Scheduler.HttpServices
{
    public class HttpInventoryService : IHttpInventoryService
    {

        private readonly IHttpCatalogueItemClient _httpCatalogueItemClient;
        private readonly IServiceResultFactory _resutlFact;

        public HttpInventoryService(IHttpCatalogueItemClient httpCatalogueItemClient, IServiceResultFactory resutlFact)
        {
            _httpCatalogueItemClient = httpCatalogueItemClient;
            _resutlFact = resutlFact;
        }





        public async Task<IServiceResult<bool>> ExistsCatalogueItemById(int itemId)
        {
            var response = await _httpCatalogueItemClient.ExistsCatalogueItemById(itemId);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result(false, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<bool>>(content);

            return result;
        }


    }
}
