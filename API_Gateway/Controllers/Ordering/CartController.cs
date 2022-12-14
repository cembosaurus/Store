using Business.Ordering.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Ordering
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly string _url;

        public CartController(IConfiguration conf)
        {
            _url = conf.GetSection("RemoteServices:OrderingService").Value + "/api/cart";
        }






        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            return new RedirectResult(url: _url, permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartById(int id)
        {
            return new RedirectResult(url: _url + $"/{id}", permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateCart(int userId)
        {
            return new RedirectResult(url: _url + $"/{userId}", permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCart(int id)
        {
            return new RedirectResult(url: _url + $"/{id}", permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            return new RedirectResult(url: _url + $"/{id}", permanent: true, preserveMethod: true);
        }


    }
}
