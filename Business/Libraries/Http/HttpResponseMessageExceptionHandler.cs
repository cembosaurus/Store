using Business.Libraries.Http.Interfaces;
using System.Net;

namespace Business.Libraries.Http
{
    public class HttpResponseMessageExceptionHandler : IHttpResponseMessageExceptionHandler
    {
        public async Task<HttpResponseMessage> Handle(HttpClient client, HttpRequestMessage message)
        {
            if (client == null || message == null)
                return new HttpResponseMessage();

            try
            {
                var result = await client.SendAsync(message);

                result.EnsureSuccessStatusCode();

                return result;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"--> {ex.StatusCode}: {ex.Message}");

                return new HttpResponseMessage(ex.StatusCode ?? HttpStatusCode.ServiceUnavailable);
            }

        }
    }
}
