using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;

namespace Ordering.HttpServices.Interfaces
{
    public interface IHttpAddressService
    {
        Task<IServiceResult<bool>> ExistsAddressByAddressId(int addressId);
        Task<IServiceResult<AddressReadDTO>> GetAddressByAddressId(int addressId);
        Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByAddressIds(IEnumerable<int> addressesIds);
    }
}
