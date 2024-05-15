/*using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Inventory.DTOs.Item;
using Business.Inventory.Http.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;



namespace Business.Inventory.Http
{
    public class HttpItemClient : HttpBaseClient, IHttpItemClient
    {     

        public HttpItemClient(IHttpContextAccessor accessor, HttpClient httpClient, IRemoteServicesInfoService remoteServicesInfoService, IExId exId)
            : base(accessor, httpClient, remoteServicesInfoService, exId)
        {
            base._remoteServiceName = "InventoryService";
            base._remoteServicePathName = "Item";
        }




        public async Task<HttpResponseMessage> GetItems(IEnumerable<int> itemIds = default)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{(itemIds != null && itemIds.Any() ? "" : "/all")}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemIds), _encoding, _mediaType);

            Console.WriteLine($"---> GETTING Items .....");

            return await TrySendAsync();
        }



        public async Task<HttpResponseMessage> GetItemById(int itemId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/{itemId}";
            //_content.Dispose();

            Console.WriteLine($"---> GETTING Item '{itemId}' ....");

            return await TrySendAsync();
        }



        public async Task<HttpResponseMessage> AddItem(ItemCreateDTO itemDTO)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemDTO), _encoding, _mediaType);

            Console.WriteLine($"---> ADDING Item '{itemDTO.Name}' ....");

            return await TrySendAsync();
        }



        public async Task<HttpResponseMessage> DeleteItem(int itemId)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"{_requestURI}/{itemId}";
            //_content.Dispose();

            Console.WriteLine($"---> DELETING Item '{itemId}' ....");

            return await TrySendAsync();
        }



        public async Task<HttpResponseMessage> UpdateItem(int itemId, ItemUpdateDTO itemDTO)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"{_requestURI}/{itemId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemDTO), _encoding, _mediaType);

            Console.WriteLine($"---> UPDATING Item '{itemId}': '{itemDTO.Name}' ....");

            return await TrySendAsync();
        }

    }
}
*/