using Business.Inventory.Http.Services.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Business.Management.Appsettings.Interfaces;
using Business.Exceptions.Interfaces;

namespace Business.Inventory.Http.Services
{
    public class HttpItemService : HttpBaseService, IHttpItemService
    {

        private readonly IServiceResultFactory _resultFact;



        public HttpItemService(IHostingEnvironment env, IExId exId, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, IRemoteServicesInfo_Provider remoteServicesInfoService)
            : base(env, exId, appsettingsService, httpAppClient, remoteServicesInfoService, resultFact)
        {
            _resultFact = resultFact;

            // get URL from local singleton
            _remoteServiceName = "InventoryService";
        }





        public async Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = default)
        {
            _remoteServicePathName = "Item";

            _method = HttpMethod.Get;
            _requestQuery = $"{(itemIds != null && itemIds.Any() ? "" : "/all")}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemIds), _encoding, _mediaType);

            var response = await Send();

            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result<IEnumerable<ItemReadDTO>>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<ItemReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int itemId)
        {
            _remoteServicePathName = "Item";

            _method = HttpMethod.Get;
            _requestQuery = $"/{itemId}";
            //_content.Dispose();

            var response = await Send();

            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result<ItemReadDTO>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO itemDTO)
        {
            _remoteServicePathName = "Item";

            _method = HttpMethod.Post;
            _requestQuery = $"";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemDTO), _encoding, _mediaType);

            var response = await Send();

            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result<ItemReadDTO>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> DeleteItem(int itemId)
        {
            _remoteServicePathName = "Item";

            _method = HttpMethod.Delete;
            _requestQuery = $"{_requestURL}/{itemId}";
            //_content.Dispose();

            var response = await Send();

            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result<ItemReadDTO>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<ItemReadDTO>> UpdateItem(int itemId, ItemUpdateDTO itemDTO)
        {
            _remoteServicePathName = "Item";

            _method = HttpMethod.Put;
            _requestQuery = $"{_requestURL}/{itemId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemDTO), _encoding, _mediaType);

            var response = await Send();

            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result<ItemReadDTO>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

            return result;
        }
    }
}





//using API_Gateway.HttpServices.Inventory.Interfaces;
//using Business.Http;
//using Business.Http.Interfaces;
//using Business.Inventory.DTOs.Item;
//using Business.Inventory.Http.Interfaces;
//using Business.Libraries.ServiceResult;
//using Business.Libraries.ServiceResult.Interfaces;
//using Business.Management.Services;
//using Business.Management.Services.Interfaces;

//namespace API_Gateway.HttpServices.Inventory
//{
//    public class HttpItemService : HttpBaseService, IHttpItemService
//    {
//        private readonly IHttpItemClient _httpItemClient;
//        private readonly IServiceResultFactory _resultFact;

//        public HttpItemService(IHttpAppClient httpAppClient, IHttpItemClient httpItemClient, IServiceResultFactory resultFact, IRemoteServicesInfoService remoteServicesInfoService)
//            : base(httpAppClient, remoteServicesInfoService)
//        {
//            _httpItemClient = httpItemClient;
//            _resultFact = resultFact;
//        }





//        public async Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = default)
//        {
//            var response = await _httpItemClient.GetItems(itemIds);

//            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
//                return _resultFact.Result<IEnumerable<ItemReadDTO>>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

//            var content = response.Content.ReadAsStringAsync().Result;

//            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<ItemReadDTO>>>(content);

//            return result;
//        }



//        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int id)
//        {
//            var response = await _httpItemClient.GetItemById(id);

//            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
//                return _resultFact.Result<ItemReadDTO>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

//            var content = response.Content.ReadAsStringAsync().Result;

//            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

//            return result;
//        }



//        public async Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO item)
//        {
//            var response = await _httpItemClient.AddItem(item);

//            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
//                return _resultFact.Result<ItemReadDTO>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

//            var content = response.Content.ReadAsStringAsync().Result;

//            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

//            return result;
//        }



//        public async Task<IServiceResult<ItemReadDTO>> DeleteItem(int id)
//        {
//            var response = await _httpItemClient.DeleteItem(id);

//            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
//                return _resultFact.Result<ItemReadDTO>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

//            var content = response.Content.ReadAsStringAsync().Result;

//            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

//            return result;
//        }



//        public async Task<IServiceResult<ItemReadDTO>> UpdateItem(int id, ItemUpdateDTO item)
//        {
//            var response = await _httpItemClient.UpdateItem(id, item);

//            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
//                return _resultFact.Result<ItemReadDTO>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

//            var content = response.Content.ReadAsStringAsync().Result;

//            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<ItemReadDTO>>(content);

//            return result;
//        }
//    }
//}
