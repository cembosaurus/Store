using Business.Identity.DTOs;
using Business.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Identity.Http.Clients.Interfaces
{
    public interface IHttpAddressClient
    {
        Task<HttpResponseMessage> AddAddressToUser(int userId, AddressCreateDTO addressDto);
        Task<HttpResponseMessage> DeleteAddress(int id);
        Task<HttpResponseMessage> ExistsAddressByAddressId(int addressId);
        Task<HttpResponseMessage> GetAddressByAddressId(int addressId);
        Task<HttpResponseMessage> GetAddressesByAddressIds(IEnumerable<int> addressesIds);
        Task<HttpResponseMessage> GetAddressesByUserId(int userId);
        Task<HttpResponseMessage> GetAllAddresses();
        Task<HttpResponseMessage> SearchAddress(SearchAddressModel searchModel);
        Task<HttpResponseMessage> UpdateAddress(int id, AddressUpdateDTO addressDto);
    }
}
