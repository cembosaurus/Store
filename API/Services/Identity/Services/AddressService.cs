using AutoMapper;
using Business.Identity.DTOs;
using Business.Identity.Models;
using Business.Libraries.ServiceResult.Interfaces;
using Identity.Data.Repositories.Interfaces;
using Identity.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Identity.Models;
using System.Collections.Generic;

namespace Identity.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepo;
        private readonly IServiceResultFactory _resultFact;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;

        public AddressService(IAddressRepository addressRepo, IServiceResultFactory resultFact, IMapper mapper, IUserRepository userRepo)
        {
            _addressRepo = addressRepo;
            _resultFact = resultFact;
            _mapper = mapper;
            _userRepo = userRepo;
        }



        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAllAddresses()
        {
            Console.WriteLine($"--> GETTING addresses ......");

            var message = "";


            var addressesResult = await _addressRepo.GetAllAddresses();

            if (!addressesResult.Any())
                message = "NO addresses found !";

            return _resultFact.Result(_mapper.Map<IEnumerable<AddressReadDTO>>(addressesResult), true, message);
        }


        public async Task<IServiceResult<AddressReadDTO>> GetAddressById(int id)
        {
            Console.WriteLine($"--> GETTING address with address Id '{id}' ......");

            var message = "";


            var address = await _addressRepo.GetAddressById(id);

            if (address == null)
                message = $"Address '{id}' NOT found !";

            return _resultFact.Result(_mapper.Map<AddressReadDTO>(address), true, message);
        }



        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByIds(IEnumerable<int> ids)
        {
            Console.WriteLine($"--> GETTING address by Ids ......");

            var message = "";


            var addresses = await _addressRepo.GetAddressesByIds(ids);

            if (addresses == null || !addresses.Any())
                message = "NO addresses corresponding to given Ids were found !";

            return _resultFact.Result(_mapper.Map<IEnumerable<AddressReadDTO>>(addresses), true, message);
        }



        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> GetAddressesByUserId(int userId)
        {
            var user = await _userRepo.GetUserById(userId);

            if (user == null)
                return _resultFact.Result<IEnumerable<AddressReadDTO>>(null, false, $"User '{userId}' NOT found !");


            Console.WriteLine($"--> GETTING addresses for user '{userId}' ......");

            var message = "";


            var addresses = await _addressRepo.GetAddressesByUserId(userId);

            if (addresses == null || !addresses.Any())
                message = $"Addresses for user '{userId}' NOT found !";

            return _resultFact.Result(_mapper.Map<IEnumerable<AddressReadDTO>>(addresses), true, message);
        }



        public async Task<IServiceResult<bool>> ExistsAddressById(int id)
        {
            Console.WriteLine($"--> CHECKING address with address Id '{id}' ......");

            var message = "";


            var addressResult = await _addressRepo.ExistsById(id);

            if (!addressResult)
                message = $"Address '{id}' does NOT exist !";

            return _resultFact.Result(addressResult, true, message);
        }


        
        public async Task<IServiceResult<AddressReadDTO>> AddAddressToUser(int userId, AddressCreateDTO addressDto)
        {
            var user = await _userRepo.GetUserById(userId);
            if(user == null)
                return _resultFact.Result<AddressReadDTO>(null, false, $"User '{userId}' NOT found !");

            if (addressDto == null)
                return _resultFact.Result<AddressReadDTO>(null, false, $"No address was entered !");


            Console.WriteLine($"--> ADDING address for user '{userId}'......");

            var message = "";


            var address = _mapper.Map<Address>(addressDto);

            var state = await _addressRepo.CreateAddress(address);

            if (state != EntityState.Added || _addressRepo.SaveChanges() < 1)
                return _resultFact.Result<AddressReadDTO>(null, false, $"Address was NOT created");


            user.UserAddresses.Add(new UserAddress { UserId = userId, AddressId = address.AddressId });

            if (_addressRepo.SaveChanges() < 1)
                return _resultFact.Result<AddressReadDTO>(null, false, $"Address: '{address.AddressId}' was NOT added to user");


            user.CurrentUsersAddress = _mapper.Map<CurrentUsersAddress>(new CurrentUsersAddress { UserId = user.Id, AddressId = address.AddressId });

            if (_addressRepo.SaveChanges() < 1)
                message = $"Address: '{user.CurrentUsersAddress.AddressId}' was NOT set as current address for user: '{user.Id}'";


            return _resultFact.Result(_mapper.Map<AddressReadDTO>(address), true, message);
        }


        // Not implemented yet:
        public async Task<IServiceResult<IEnumerable<AddressReadDTO>>> SearchAddress(SearchAddressModel searchModel)
        {
            Console.WriteLine($"--> SEARCHING for addresses by parameters ......");

            var message = "";


            // To Do: implement search functionality
            var addresses = await _addressRepo.SearchAddress(searchModel);

            if (!addresses.Any())
                message = "NO addresses found !";

            return _resultFact.Result(_mapper.Map<IEnumerable<AddressReadDTO>>(addresses), true, message);
        }


        public async Task<IServiceResult<AddressReadDTO>> UpdateAddress(int id, AddressUpdateDTO addressDto)
        {
            if(addressDto == null)
                return _resultFact.Result<AddressReadDTO>(null, false, $"Address was NOT provided !");
            if(string.IsNullOrWhiteSpace(addressDto.City) || string.IsNullOrWhiteSpace(addressDto.Street) || addressDto.Number < 1)
                return _resultFact.Result<AddressReadDTO>(null, false, $"Correct address details was NOT provided !");

            var address = await _addressRepo.GetAddressById(id);

            if(address == null)
                return _resultFact.Result<AddressReadDTO>(null, false, $"Address NOT found !");


            Console.WriteLine($"--> UPDATING address: '{address.AddressId}' ......");


            address = _mapper.Map<Address>(addressDto);

            if (_addressRepo.SaveChanges() < 1)
                return _resultFact.Result<AddressReadDTO>(null, false, $"Address: '{address.AddressId}' was NOT updated !");

            return _resultFact.Result(_mapper.Map<AddressReadDTO>(address), true);
        }


        public async Task<IServiceResult<AddressReadDTO>> DeleteAddress(int id)
        {
            var address = await _addressRepo.GetAddressById(id);

            if (address == null)
                return _resultFact.Result<AddressReadDTO>(null, false, $"Address with Id '{id}' NOT found !");


            Console.WriteLine($"--> DELETING address '{address.AddressId}' ......");


            var state = await _addressRepo.DeleteAddress(address);

            if (state != EntityState.Deleted || _addressRepo.SaveChanges() < 1)
                return _resultFact.Result<AddressReadDTO>(null, false, $"Address with id '{id}' was NOT removed from DB !");

            return _resultFact.Result(_mapper.Map<AddressReadDTO>(address), true);
        }


    }
}
