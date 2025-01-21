using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;



namespace API_Gateway.Controllers.Business.StaticContent
{
    [Route("[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {

        private IGlobalConfig_PROVIDER _globalConfig_Provider;


        public PhotosController(IGlobalConfig_PROVIDER globalConfig_Provider)
        {
            _globalConfig_Provider = globalConfig_Provider;
        }




        [HttpGet("items/{id}")]
        public async Task<object> GetById(string id)
        {
            var urlResult = _globalConfig_Provider.GetRemoteServiceURL_WithPath("StaticContentService", "ItemsURL");

            if (!urlResult.Status)
                return StatusCode(StatusCodes.Status204NoContent, urlResult);

            // f.e: http://localhost:4000/photos/items/onion.jpg
            return Redirect($"{urlResult.Data}/{id}");
        }



    }
}
