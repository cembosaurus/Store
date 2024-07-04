using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services;
using Business.Identity.DTOs;
using Business.Identity.Http.Services.Interfaces;
using Business.Identity.Models;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Business.Identity.Http.Services
{
    public class HttpAddressService : HttpBaseService, IHttpAddressService
    {


        public HttpAddressService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact)
            : base(accessor, env, exId, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "IdentityService";
            _remoteServicePathName = "Address";
        }





        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAllAddresses()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"all";

            return await HTTP_Request_Handler<IEnumerable<AddressReadDTO>>();
        }


        public async Task<IServiceResult<bool>> ExistsAddressByAddressId(int addressId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{addressId}/exists";

            return await HTTP_Request_Handler<bool>();
        }


        public async Task<IServiceResult<AddressReadDTO>> GetAddressByAddressId(int addressId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"{addressId}";

            return await HTTP_Request_Handler<AddressReadDTO>();
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByAddressIds(IEnumerable<int> addressesIds)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressesIds), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<AddressReadDTO>>();
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByUserId(int userId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"user/{userId}";

            return await HTTP_Request_Handler<IEnumerable<AddressReadDTO>>();
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> SearchAddress(SearchAddressModel searchModel)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(searchModel), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<AddressReadDTO>>();
        }


        public async Task<IServiceResult<AddressReadDTO>> UpdateAddress(int id, AddressUpdateDTO addressDto)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"{id}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressDto), _encoding, _mediaType);

            return await HTTP_Request_Handler<AddressReadDTO>();
        }


        public async Task<IServiceResult<AddressReadDTO>> AddAddressToUser(int userId, AddressCreateDTO addressDto)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"{userId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressDto), _encoding, _mediaType);

            return await HTTP_Request_Handler<AddressReadDTO>();
        }


        public async Task<IServiceResult<AddressReadDTO>> DeleteAddress(int id)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"{id}";

            return await HTTP_Request_Handler<AddressReadDTO>();
        }
    }
}
