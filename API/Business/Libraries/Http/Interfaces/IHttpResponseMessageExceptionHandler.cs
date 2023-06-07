namespace Business.Libraries.Http.Interfaces
{
    public interface IHttpResponseMessageExceptionHandler
    {
        Task<HttpResponseMessage> Handle(HttpClient client, HttpRequestMessage message);
    }
}
