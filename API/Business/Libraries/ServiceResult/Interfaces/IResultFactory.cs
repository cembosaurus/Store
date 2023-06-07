namespace Business.Libraries.ServiceResult.Interfaces
{
    public interface IServiceResultFactory
    {
        IServiceResult Result(bool status = false, string message = "");
        IServiceResult<T> Result<T>(T? data, bool status = false, string message = "");
    }

}
