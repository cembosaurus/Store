namespace Business.Http.Interfaces
{
    public interface IHttpAppClient
    {
        Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage);
    }
}
