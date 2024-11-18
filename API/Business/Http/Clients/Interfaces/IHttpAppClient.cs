namespace Business.Http.Clients.Interfaces
{
    public interface IHttpAppClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, bool bypassMetrics = default);
    }
}