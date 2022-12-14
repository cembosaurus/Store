using Business.Identity.DTOs;
using Business.Identity.Http.Clients.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Ordering.HttpServices.Interfaces;

namespace Ordering.HttpServices
{
    public class HttpAddressService : IHttpAddressService
    {
        private readonly IHttpAddressClient _httpAddressClient;
        private readonly IServiceResultFactory _resutlFact;

        public HttpAddressService(IHttpAddressClient httpAddressClient, IServiceResultFactory resutlFact)
        {
            _httpAddressClient = httpAddressClient;
            _resutlFact = resutlFact;
        }





        public async Task<IServiceResult<AddressReadDTO>> GetAddressByAddressId(int addressId)
        {
            var response = await _httpAddressClient.GetAddressByAddressId(addressId);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<AddressReadDTO>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var addressToReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<AddressReadDTO>>(content);

            return addressToReturn;
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByAddressIds(IEnumerable<int> addressesIds)
        {
            var response = await _httpAddressClient.GetAddressesByAddressIds(addressesIds);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result<IEnumerable<AddressReadDTO>>(null, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var addressesToReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<AddressReadDTO>>>(content);

            return addressesToReturn;
        }



        public async Task<IServiceResult<bool>> ExistsAddressByAddressId(int addressId)
        {
            var response = await _httpAddressClient.ExistsAddressByAddressId(addressId);

            if (!response.IsSuccessStatusCode)
                return _resutlFact.Result(false, false, response.StatusCode.ToString());

            var content = response.Content.ReadAsStringAsync().Result;

            var addressExists = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<bool>>(content);

            return addressExists;
        }

    }
}
