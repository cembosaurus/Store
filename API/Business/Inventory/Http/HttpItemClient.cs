﻿using Business.Inventory.DTOs.Item;
using Business.Inventory.Http.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Inventory.Http
{
    public class HttpItemClient : IHttpItemClient
    {

        private HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";


        public HttpItemClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:AMQP:InventoryService").Value + "/api/item";
            _accessor = accessor;
        }




        public async Task<HttpResponseMessage> GetItems(IEnumerable<int> itemIds = default)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"{(itemIds != null && itemIds.Any() ? "" : "/all")}",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemIds), _encoding, _mediaType)
            );

            Console.WriteLine($"---> GETTING Items .....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> GetItemById(int itemId)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Get,
                $"/{itemId}"
            );

            Console.WriteLine($"---> GETTING Item '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> AddItem(ItemCreateDTO itemDTO)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Post,
                $"",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemDTO), _encoding, _mediaType)
            );

            Console.WriteLine($"---> ADDING Item '{itemDTO.Name}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> DeleteItem(int itemId)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Delete,
                $"{_baseUri}/{itemId}"
            );

            Console.WriteLine($"---> DELETING Item '{itemId}' ....");

            return await _httpClient.SendAsync(_request);
        }



        public async Task<HttpResponseMessage> UpdateItem(int itemId, ItemUpdateDTO itemDTO)
        {
            InitializeHttpRequestMessage(
                HttpMethod.Put,
                $"{_baseUri}/{itemId}",
                new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(itemDTO), _encoding, _mediaType)
            );

            Console.WriteLine($"---> UPDATING Item '{itemId}': '{itemDTO.Name}' ....");

            return await _httpClient.SendAsync(_request);
        }





        private void InitializeHttpRequestMessage(HttpMethod method, string uri, HttpContent content = default)
        {
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri + uri) };
            _request.Method = method;
            _request.Content = content;
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

    }
}
