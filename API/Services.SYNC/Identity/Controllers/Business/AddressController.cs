using Business.Identity.DTOs;
using Business.Identity.Models;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers.Business
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }





        [Authorize(Policy = "Everyone")]
        [HttpGet("all")]
        public async Task<object> GetAllAddresses()
        {
            var result = await _addressService.GetAllAddresses();

            return result;  // ctr res
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}")]
        public async Task<object> GetAddressById(int id)
        {
            var result = await _addressService.GetAddressById(id);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet()]
        public async Task<object> GetAddressesByIds(IEnumerable<int> ids)
        {
            var result = await _addressService.GetAddressesByIds(ids);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("user/{userId}")]
        public async Task<object> GetAddressesByUserId(int userId)
        {
            var result = await _addressService.GetAddressesByUserId(userId);

            return result;  // ctr res
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("search")]
        public async Task<object> SearchAddress(SearchAddressModel searchModel)
        {
            var result = await _addressService.SearchAddress(searchModel);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}/exists")]
        public async Task<object> ExistsAddressById(int id)
        {
            var result = await _addressService.ExistsAddressById(id);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost("{userId}")]
        public async Task<object> AddAddressToUser(int userId, AddressCreateDTO address)
        {
            var result = await _addressService.AddAddressToUser(userId, address);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{id}")]
        public async Task<object> UpdateAddress(int id, AddressUpdateDTO address)
        {
            var result = await _addressService.UpdateAddress(id, address);

            return result;  // ctr res
        }


        [Authorize(Policy = "Everyone")]
        [HttpDelete("{id}")]
        public async Task<object> DeleteAddress(int id)
        {
            var result = await _addressService.DeleteAddress(id);

            return result;  // ctr res
        }



    }
}
