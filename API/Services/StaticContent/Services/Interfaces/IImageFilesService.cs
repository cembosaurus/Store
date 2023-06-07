namespace StaticContent.Services.Interfaces
{
    public interface IImageFilesService
    {
        FileStream GetById(string id);
    }
}
