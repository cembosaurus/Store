using Business.Identity.DTOs;
using Business.Identity.Models;
using Business.Libraries.ServiceResult.Interfaces;

namespace Identity.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IServiceResult<AddressReadDTO>> AddAddressToUser(int userId, AddressCreateDTO addressDto);
        Task<IServiceResult<AddressReadDTO>> DeleteAddress(int id);
        Task<IServiceResult<bool>> ExistsAddressById(int id);
        Task<IServiceResult<AddressReadDTO>> GetAddressById(int id);
        Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByIds(IEnumerable<int> ids);
        Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByUserId(int userId);
        Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAllAddresses();
        Task<IServiceResult<IEnumerable<AddressReadDTO>>> SearchAddress(SearchAddressModel searchModel);
        Task<IServiceResult<AddressReadDTO>> UpdateAddress(int id, AddressUpdateDTO addressDto);
    }
}
