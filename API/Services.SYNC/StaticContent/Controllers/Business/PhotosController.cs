using Microsoft.AspNetCore.Mvc;
using StaticContent.Services.Interfaces;



namespace StaticContent.Controllers.Business
{

    [Route("[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {

        private readonly IImageFilesService _imageFilesService;


        public PhotosController(IImageFilesService imageFilesService)
        {
            _imageFilesService = imageFilesService;
        }




        [HttpGet("items/{id}")]
        public async Task<object> GetById(string id)
        {
            var image = _imageFilesService.GetById(id);

            return File(image, "image/jpeg");
        }


    }
}
