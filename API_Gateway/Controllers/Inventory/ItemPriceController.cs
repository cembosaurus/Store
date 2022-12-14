using Business.Inventory.DTOs.ItemPrice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Inventory
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemPriceController : ControllerBase
    {

        private readonly string _url;


        public ItemPriceController(IConfiguration conf)
        {
            _url = conf.GetSection("RemoteServices:InventoryService").Value + "/api/ItemPrice";
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<ActionResult> GetAllItemPrices()
        {
            return new RedirectResult(url: _url, permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}")]
        public async Task<ActionResult> GetItemPriceById(int itemId)
        {
            return new RedirectResult(url: _url + $"/{itemId}", permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{itemId}")]
        public async Task<ActionResult> EditItemPrice(int itemId)
        {
            return new RedirectResult(url: _url + $"/{itemId}", permanent: true, preserveMethod: true);
        }

    }
}
