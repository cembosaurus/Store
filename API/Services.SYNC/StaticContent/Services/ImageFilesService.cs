using StaticContent.Services.Interfaces;

namespace StaticContent.Services
{
    public class ImageFilesService: IImageFilesService
    {

        private readonly IConfiguration _conf;
        private readonly string _imagePath;



        public ImageFilesService(IConfiguration conf)
        {
            _conf = conf;

            // Images will be moved into DB:
            _imagePath = _conf.GetSection("Config:Local:Images").Value;
        }



        public FileStream GetById(string id)
        {
            var image = File.OpenRead(_imagePath + id);

            //image.Close();

            return image;
        }


    }
}
