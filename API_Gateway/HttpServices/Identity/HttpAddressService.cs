using API_Gateway.HttpServices.Identity.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Http.Clients.Interfaces;
using Business.Identity.Models;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.HttpServices.Identity
{
    public class HttpAddressService : IHttpAddressService
    {

        private readonly IHttpAddressClient _httpAddressClient;

        public HttpAddressService(IHttpAddressClient httpAddressClient)
        {
            _httpAddressClient = httpAddressClient;
        }





        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAllAddresses()
        {
            var response = await _httpAddressClient.GetAllAddresses();

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<AddressReadDTO>>>(content);

            return result;
        }


        public async Task<IServiceResult<bool>> ExistsAddressByAddressId(int addressId)
        {
            var response = await _httpAddressClient.ExistsAddressByAddressId(addressId);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<bool>>(content);

            return result;
        }


        public async Task<IServiceResult<AddressReadDTO>> GetAddressByAddressId(int addressId)
        {
            var response = await _httpAddressClient.GetAddressByAddressId(addressId);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<AddressReadDTO>>(content);

            return result;
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByAddressIds(IEnumerable<int> addressesIds)
        {
            var response = await _httpAddressClient.GetAddressesByAddressIds(addressesIds);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<AddressReadDTO>>>(content);

            return result;
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByUserId(int userId)
        {
            var response = await _httpAddressClient.GetAddressesByUserId(userId);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<AddressReadDTO>>>(content);

            return result;
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> SearchAddress(SearchAddressModel searchModel)
        {
            var response = await _httpAddressClient.SearchAddress(searchModel);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<AddressReadDTO>>>(content);

            return result;
        }


        public async Task<IServiceResult<AddressReadDTO>> UpdateAddress(int id, AddressUpdateDTO addressDto)
        {
            var response = await _httpAddressClient.UpdateAddress(id, addressDto);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<AddressReadDTO>>(content);

            return result;
        }


        public async Task<IServiceResult<AddressReadDTO>> AddAddressToUser(int userId, AddressCreateDTO addressDto)
        {
            var response = await _httpAddressClient.AddAddressToUser(userId, addressDto);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<AddressReadDTO>>(content);

            return result;
        }


        public async Task<IServiceResult<AddressReadDTO>> DeleteAddress(int id)
        {
            var response = await _httpAddressClient.DeleteAddress(id);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<AddressReadDTO>>(content);

            return result;
        }
    }
}
