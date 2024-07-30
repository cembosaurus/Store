namespace Business.Metrics.Http.Clients.Interfaces
{
    public interface IHttpClient_Metrics
    {
        HttpClient HtpClient { get; }
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage);
    }
}
