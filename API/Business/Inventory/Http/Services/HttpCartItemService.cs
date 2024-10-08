﻿using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services;
using Business.Http.Services.Interfaces;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Business.Ordering.DTOs;
using Business.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;



namespace Business.Inventory.Http.Services
{
    public class HttpCartItemService : HttpBaseService, IHttpBaseService, IHttpCartItemService
    {

        public HttpCartItemService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact, ConsoleWriter cm)
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact, cm)
        {
            _remoteServiceName = "OrderingService";
            _remoteServicePathName = "Cart";
        }





        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> AddItemsToCart(int userId, IEnumerable<CartItemUpdateDTO> items)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"{userId}/items";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<CartItemReadDTO>>();
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> DeleteCartItems(int userId, IEnumerable<int> items)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"{userId}/items/delete";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<CartItemReadDTO>>();
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetAllCartsItems()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"items/all";

            return await HTTP_Request_Handler<IEnumerable<CartItemReadDTO>>();
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetCartItems(int userId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{userId}/items";

            return await HTTP_Request_Handler<IEnumerable<CartItemReadDTO>>();
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> RemoveCartItems(int userId, IEnumerable<CartItemUpdateDTO> items)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"{userId}/items";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { items }), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<CartItemReadDTO>>();
        }
    }
}
