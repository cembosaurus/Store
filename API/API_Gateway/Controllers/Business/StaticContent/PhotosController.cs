using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace API_Gateway.Controllers.Business.StaticContent
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly string _staticContentBaseUrl;
        private readonly string _staticContentItemsUrl;

        public PhotosController(IConfiguration config)
        {
            _staticContentBaseUrl = config.GetSection("RemoteServices:StaticContentService:REST:BaseURL").Value;
            _staticContentItemsUrl = config.GetSection("RemoteServices:StaticContentService:REST:ItemsURL").Value;
        }


        //[Authorize(Policy = "Everyone")]
        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetById(string id)
        {






            // f.e: http://localhost:4000  /  api  /  photos/items  /  onion.jpg

            return Redirect($"{_staticContentBaseUrl}/{_staticContentItemsUrl}/{id}");





        }



    }
}
