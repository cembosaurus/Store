using Business.Inventory.DTOs.CatalogueItem;
using Business.Libraries.ServiceResult.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Inventory
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogueItemController : ControllerBase
    {

        private readonly string _url;


        public CatalogueItemController(IConfiguration conf)
        {
            _url = conf.GetSection("RemoteServices:InventoryService").Value + "/api/CatalogueItem";
        }




        // GET:

        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<ActionResult> GetAllCatalogueItems()
        {
            return new RedirectResult(url: _url, permanent: true, preserveMethod: true);
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}")]
        public async Task<ActionResult> GetCatalogueItemById(int itemId)
        {
            return new RedirectResult(url: _url + $"/{itemId}", permanent: true, preserveMethod: true);
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("{itemId}/extras")]
        public async Task<ActionResult> GetCatalogueItemWithExtrasById(int itemId)
        {
            return new RedirectResult(url: _url + $"/{itemId}/extras", permanent: true, preserveMethod: true);
        }



        // POST:

        [Authorize(Policy = "Everyone")]
        [HttpPost("{itemId}")]
        public async Task<ActionResult> CreateCatalogueItem(int itemId)
        {
            return new RedirectResult(url: _url + $"/{itemId}", permanent: true, preserveMethod: true);
        }




        [Authorize(Policy = "Everyone")]
        [HttpPost("{itemId}/extras")]
        public async Task<ActionResult> AddExtrasToCatalogueItem(int itemId)
        {
            return new RedirectResult(url: _url + $"/{itemId}/extras", permanent: true, preserveMethod: true);
        }



        // PUT:

        [Authorize(Policy = "Everyone")]
        [HttpPut("{itemId}")]
        public async Task<ActionResult> UpdateCatalogueItem(int itemId)
        {
            return new RedirectResult(url: _url + $"/{itemId}", permanent: true, preserveMethod: true);
        }



        // DELETE:

        [Authorize(Policy = "Everyone")]
        [HttpDelete("{itemId}")]
        public async Task<ActionResult> RemoveCatalogueItem(int itemId)
        {
            return new RedirectResult(url: _url + $"/{itemId}", permanent: true, preserveMethod: true);
        }


        [Authorize(Policy = "Everyone")]
        [HttpDelete("{itemId}/extras")]
        public async Task<ActionResult> RemoveExtrasFromCatalogueItem(int itemId)
        {
            return new RedirectResult(url: _url + $"/{itemId}/extras", permanent: true, preserveMethod: true);
        }
    }
}
