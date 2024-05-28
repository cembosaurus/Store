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
        public async Task<IActionResult> GetAllAddresses()
        {
            var result = await _addressService.GetAllAddresses();

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressByAddressId(int id)
        {
            var result = await _addressService.GetAddressByAddressId(id);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet()]
        public async Task<IActionResult> GetAddressesByAddressIds(IEnumerable<int> ids)
        {
            var result = await _addressService.GetAddressesByAddressIds(ids);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAddressesByUserId(int userId)
        {
            var result = await _addressService.GetAddressesByUserId(userId);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchAddress(SearchAddressModel searchModel)
        {
            var result = await _addressService.SearchAddress(searchModel);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}/exists")]
        public async Task<IActionResult> AddressExistsByAddressId(int id)
        {
            var result = await _addressService.ExistsAddressByAddressId(id);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateAddress(int userId, AddressCreateDTO address)
        {
            var result = await _addressService.AddAddressToUser(userId, address);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, AddressUpdateDTO address)
        {
            var result = await _addressService.UpdateAddress(id, address);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var result = await _addressService.DeleteAddress(id);

            return result.Status ? Ok(result) : BadRequest(result);
        }



    }
}
