namespace Business.Libraries.ServiceResult.Interfaces
{
    public interface IServiceResult<T>
    {
        T? Data { get; }
        string Message { get; }
        bool Status { get; }
    }

    public interface IServiceResult
    {
        string Message { get; }
        bool Status { get; }
    }
}
