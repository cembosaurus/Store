﻿using Microsoft.AspNetCore.Mvc;
using StaticContent.Services.Interfaces;



namespace StaticContent.Controllers.Business
{

    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IImageFilesService _imageFilesService;


        public PhotosController(IImageFilesService imageFilesService)
        {
            _imageFilesService = imageFilesService;
        }




        //[Authorize(Policy = "Everyone")]
        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var x = Request.Headers["Authorization"];


            var image = _imageFilesService.GetById(id);

            return File(image, "image/jpeg");
        }



    }
}
