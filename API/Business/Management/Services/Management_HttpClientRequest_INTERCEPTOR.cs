namespace Business.Management.Services
{
    public class Management_HttpClientRequest_INTERCEPTOR : DelegatingHandler
    {

        private HttpResponseMessage _responseMessage;




        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {


            // To Do: move 503 re-call logic here from HttpBaseServicve !!!!!!




            _responseMessage = await base.SendAsync(requestMessage, cancellationToken);

            return _responseMessage;
        }




    }
}
