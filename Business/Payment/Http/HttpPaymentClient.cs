using Business.Libraries.Http.Interfaces;
using Business.Ordering.DTOs;
using Business.Payment.DTOs;
using Business.Payment.Http.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Payment.Http
{
    public class HttpPaymentClient : IHttpPaymentClient
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";


        public HttpPaymentClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:PaymentService").Value + "/api/payment";
            _accessor = accessor;
        }





        public async Task<HttpResponseMessage> MakePayment(OrderPaymentCreateDTO order)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_baseUri),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json")
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            Console.WriteLine($"---> SENDING payment request .....");

            return await _httpClient.SendAsync(request);
        }


    }
}
