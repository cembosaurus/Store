using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Ordering
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly string _url;



        public OrderController(IConfiguration conf)
        {
            _url = conf.GetSection("RemoteServices:OrderingService").Value + "/api/order";
        }




        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            return new RedirectResult(url: _url, permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            return new RedirectResult(url: _url + $"/{id}", permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPost("{cartId}")]
        public async Task<IActionResult> CreateOrder(int cartId)
        {
            return new RedirectResult(url: _url + $"/{cartId}", permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            return new RedirectResult(url: _url + $"/{id}", permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return new RedirectResult(url: _url + $"/{id}", permanent: true, preserveMethod: true);
        }

    }
}
