using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaticContent.Services.Interfaces;

namespace StaticContent.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IImageFilesService _imageFilesService;


        public PhotosController(IImageFilesService imageFilesService)
        {
            _imageFilesService = imageFilesService;
        }




        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var image = _imageFilesService.GetById(id);

            return File(image, "image/jpeg");
        }

        

    }
}
