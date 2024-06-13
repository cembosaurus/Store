using Business.Exceptions.Interfaces;
using Business.Http;
using Business.Http.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Http.Services.Interfaces;
using Business.Identity.Models;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;



namespace Business.Identity.Http.Services
{
    public class HttpAddressService : HttpBaseService, IHttpAddressService
    {


        public HttpAddressService(IHostingEnvironment env, IExId exId, IAppsettings_Provider appsettingsService, IHttpAppClient httpAppClient, IRemoteServices_Provider remoteServices_Provider, IServiceResultFactory resultFact)
            : base(env, exId, appsettingsService, httpAppClient, remoteServices_Provider, resultFact)
        {
            _remoteServiceName = "IdentityService";
        }





        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAllAddresses()
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/address/all";

            return await HTTP_Request_Handler<IEnumerable<AddressReadDTO>>();
        }


        public async Task<IServiceResult<bool>> ExistsAddressByAddressId(int addressId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/address/{addressId}/exists";

            return await HTTP_Request_Handler<bool>();
        }


        public async Task<IServiceResult<AddressReadDTO>> GetAddressByAddressId(int addressId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/address/{addressId}";

            return await HTTP_Request_Handler<AddressReadDTO>();
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByAddressIds(IEnumerable<int> addressesIds)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/address";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressesIds), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<AddressReadDTO>>();
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByUserId(int userId)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/address/user/{userId}";

            return await HTTP_Request_Handler<IEnumerable<AddressReadDTO>>();
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> SearchAddress(SearchAddressModel searchModel)
        {
            _method = HttpMethod.Get;
            _requestQuery = $"/address";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(searchModel), _encoding, _mediaType);

            return await HTTP_Request_Handler<IEnumerable<AddressReadDTO>>();
        }


        public async Task<IServiceResult<AddressReadDTO>> UpdateAddress(int id, AddressUpdateDTO addressDto)
        {
            _method = HttpMethod.Put;
            _requestQuery = $"/address/{id}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressDto), _encoding, _mediaType);

            return await HTTP_Request_Handler<AddressReadDTO>();
        }


        public async Task<IServiceResult<AddressReadDTO>> AddAddressToUser(int userId, AddressCreateDTO addressDto)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/address/{userId}";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addressDto), _encoding, _mediaType);

            return await HTTP_Request_Handler<AddressReadDTO>();
        }


        public async Task<IServiceResult<AddressReadDTO>> DeleteAddress(int id)
        {
            _method = HttpMethod.Delete;
            _requestQuery = $"/address/{id}";

            return await HTTP_Request_Handler<AddressReadDTO>();
        }
    }
}
