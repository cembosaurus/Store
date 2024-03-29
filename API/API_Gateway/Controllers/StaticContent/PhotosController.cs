﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace API_Gateway.Controllers.StaticContent
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
            _staticContentBaseUrl = config.GetSection("RemoteServices:StaticContentService:BaseURL").Value;
            _staticContentItemsUrl = config.GetSection("RemoteServices:StaticContentService:ItemsURL").Value;
        }


        //[Authorize(Policy = "Everyone")]
        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Redirect($"{_staticContentBaseUrl}/{_staticContentItemsUrl}/{id}");
        }



    }
}
