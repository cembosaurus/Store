using API_Gateway.Services.Business.Identity.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Http.Services.Interfaces;
using Business.Identity.Models;
using Business.Libraries.ServiceResult.Interfaces;



namespace API_Gateway.Services.Business.Identity
{
    public class AddressService : IAddressService
    {

        private readonly IHttpAddressService _httpAddressService;

        public AddressService(IHttpAddressService httpAddressService)
        {
            _httpAddressService = httpAddressService;
        }



        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAllAddresses()
        {
            return await _httpAddressService.GetAllAddresses();
        }


        public async Task<IServiceResult<bool>> ExistsAddressByAddressId(int addressId)
        {
            return await _httpAddressService.ExistsAddressByAddressId(addressId);
        }


        public async Task<IServiceResult<AddressReadDTO>> GetAddressByAddressId(int addressId)
        {
            return await _httpAddressService.GetAddressByAddressId(addressId);
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByAddressIds(IEnumerable<int> addressesIds)
        {
            return await _httpAddressService.GetAddressesByAddressIds(addressesIds);
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByUserId(int userId)
        {
            return await _httpAddressService.GetAddressesByUserId(userId);
        }


        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> SearchAddress(SearchAddressModel searchModel)
        {
            return await _httpAddressService.SearchAddress(searchModel);
        }


        public async Task<IServiceResult<AddressReadDTO>> UpdateAddress(int id, AddressUpdateDTO addressDto)
        {
            return await _httpAddressService.UpdateAddress(id, addressDto);
        }


        public async Task<IServiceResult<AddressReadDTO>> AddAddressToUser(int userId, AddressCreateDTO addressDto)
        {
            return await _httpAddressService.AddAddressToUser(userId, addressDto);
        }


        public async Task<IServiceResult<AddressReadDTO>> DeleteAddress(int id)
        {
            return await _httpAddressService.DeleteAddress(id);
        }
    }
}
