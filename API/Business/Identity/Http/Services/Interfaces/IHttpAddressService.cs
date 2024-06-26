﻿using Business.Identity.DTOs;
using Business.Identity.Models;
using Business.Libraries.ServiceResult.Interfaces;

namespace Business.Identity.Http.Services.Interfaces
{
    public interface IHttpAddressService
    {
        Task<IServiceResult<bool>> ExistsAddressByAddressId(int addressId);
        Task<IServiceResult<AddressReadDTO>> GetAddressByAddressId(int addressId);
        Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByAddressIds(IEnumerable<int> addressesIds);
        Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByUserId(int userId);
        Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAllAddresses();
        Task<IServiceResult<IEnumerable<AddressReadDTO>>> SearchAddress(SearchAddressModel searchModel);
        Task<IServiceResult<AddressReadDTO>> UpdateAddress(int id, AddressUpdateDTO addressDto);
        Task<IServiceResult<AddressReadDTO>> AddAddressToUser(int userId, AddressCreateDTO addressDto);
        Task<IServiceResult<AddressReadDTO>> DeleteAddress(int id);
    }
}
