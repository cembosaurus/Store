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

        private readonly HttpRequestMessage _request;
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _mediaType = "application/json";

        private static IHttpContextAccessor _accessor;
        private static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";


        public HttpPaymentClient(HttpClient httpClient, IConfiguration config, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _baseUri = config.GetSection("RemoteServices:PaymentService").Value + "/api/payment";
            _accessor = accessor;
            _request = new HttpRequestMessage { RequestUri = new Uri(_baseUri) };
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }





        public async Task<HttpResponseMessage> MakePayment(OrderPaymentCreateDTO order)
        {
            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_request.RequestUri + $"");
            _request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(order), _encoding, _mediaType);

            Console.WriteLine($"---> SENDING payment request .....");

            return await _httpClient.SendAsync(_request);
        }


    }
}
