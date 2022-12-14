using Business.Data.Repositories.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Models;
using Business.Libraries.ServiceResult.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Identity.Models;

namespace Identity.Data.Repositories.Interfaces
{
    public interface IAddressRepository : IBaseRepository
    {
        Task<EntityEntry> CreateAddress(Address address);
        Task<EntityEntry> DeleteAddress(Address address);
        Task<bool> ExistsById(int id);
        Task<Address> GetAddressById(int id);
        Task<IEnumerable<Address>> GetAddressesByIds(IEnumerable<int> ids);
        Task<IEnumerable<Address>> GetAddressesByUserId(int userId);
        Task<IEnumerable<Address>> GetAllAddresses();
        Task<IEnumerable<Address>> SearchAddress(SearchAddressModel searchModel);

        //Task<Address> GetAddressByCity(int id);
        //Task<Address> GetAddressByStreet(string id);
        //Task<Address> GetAddressByStreetNumber(int id);
    }
}
