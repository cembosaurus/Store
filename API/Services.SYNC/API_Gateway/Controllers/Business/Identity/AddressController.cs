using API_Gateway.Services.Business.Identity.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Business.Identity
{
    [Route("[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {

        private readonly string _url;
        private readonly IAddressService _addressService;


        public AddressController(IConfiguration conf, IAddressService addressService)
        {
            _addressService = addressService;
            _url = conf.GetSection("RemoteServices:IdentityService:REST:BaseURL").Value + "/api/identity";

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
        public async Task<object> GetAddressByAddressId(int id)
        {
            var result = await _addressService.GetAddressByAddressId(id);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet()]
        public async Task<object> GetAddressesByAddressIds(IEnumerable<int> ids)
        {
            var result = await _addressService.GetAddressesByAddressIds(ids);

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
        public async Task<object> AddressExistsByAddressId(int id)
        {
            var result = await _addressService.ExistsAddressByAddressId(id);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost("{userId}")]
        public async Task<object> CreateAddress(int userId, AddressCreateDTO address)
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
