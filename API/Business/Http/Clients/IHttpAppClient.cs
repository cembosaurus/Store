namespace Business.Http.Clients
{
    public interface IHttpAppClient
    {
        Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage);
    }
}
