namespace Business.Libraries.ServiceResult.Interfaces
{
    public interface IServiceResult<T>
    {
        T? Data { get; }
        string Message { get; }
        bool Status { get; }
        ServiceResult AppendMessage(string msg = "");
        ServiceResult PrependMessage(string msg = "");
    }

    public interface IServiceResult
    {
        string Message { get; }
        bool Status { get; }
        ServiceResult AppendMessage(string msg = "");
        ServiceResult PrependMessage(string msg = "");
    }
}
