namespace Business.Http.Clients.Interfaces
{
    public interface IHttpAppClient
    {
        Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage);
    }
}
