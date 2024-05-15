using Business.Identity.DTOs;
using Business.Identity.Http.Clients.Interfaces;
using Business.Identity.Http.Services.Interfaces;
using Business.Identity.Models;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;



namespace Business.Identity.Http.Services
{
    public class HttpAddressService : IHttpAddressService
    {

        private readonly IHttpAddressClient _httpAddressClient;
        private readonly IServiceResultFactory _resultFact;

        public HttpAddressService(IHttpAddressClient httpAddressClient, IServiceResultFactory resultFact)
        {
            _httpAddressClient = httpAddressClient;
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAllAddresses()
        {
            var response = await _httpAddressClient.GetAllAddresses();

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<AddressReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<AddressReadDTO>>>(content);

            return result;
        }


        public async Task<IServiceResult<bool>> ExistsAddressByAddressId(int addressId)
        {
            var response = await _httpAddressClient.ExistsAddressByAddressId(addressId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result(false, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<bool>>(content);

            return result;
        }


        public async Task<IServiceResult<AddressReadDTO>> GetAddressByAddressId(int addressId)
        {
            var response = await _httpAddressClient.GetAddressByAddressId(addressId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<AddressReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<AddressReadDTO>>(content);

            return result;
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByAddressIds(IEnumerable<int> addressesIds)
        {
            var response = await _httpAddressClient.GetAddressesByAddressIds(addressesIds);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<AddressReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<AddressReadDTO>>>(content);

            return result;
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByUserId(int userId)
        {
            var response = await _httpAddressClient.GetAddressesByUserId(userId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<AddressReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<AddressReadDTO>>>(content);

            return result;
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> SearchAddress(SearchAddressModel searchModel)
        {
            var response = await _httpAddressClient.SearchAddress(searchModel);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<AddressReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<AddressReadDTO>>>(content);

            return result;
        }


        public async Task<IServiceResult<AddressReadDTO>> UpdateAddress(int id, AddressUpdateDTO addressDto)
        {
            var response = await _httpAddressClient.UpdateAddress(id, addressDto);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<AddressReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<AddressReadDTO>>(content);

            return result;
        }


        public async Task<IServiceResult<AddressReadDTO>> AddAddressToUser(int userId, AddressCreateDTO addressDto)
        {
            var response = await _httpAddressClient.AddAddressToUser(userId, addressDto);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<AddressReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<AddressReadDTO>>(content);

            return result;
        }


        public async Task<IServiceResult<AddressReadDTO>> DeleteAddress(int id)
        {
            var response = await _httpAddressClient.DeleteAddress(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<AddressReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<AddressReadDTO>>(content);

            return result;
        }
    }
}
