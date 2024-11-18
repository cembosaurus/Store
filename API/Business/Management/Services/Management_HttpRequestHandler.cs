using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Management.Services
{
    public class Management_HttpRequestHandler : DelegatingHandler
    {

        private HttpResponseMessage _responseMessage;




        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            _responseMessage = await base.SendAsync(requestMessage, cancellationToken);

            return _responseMessage;
        }




    }
}
