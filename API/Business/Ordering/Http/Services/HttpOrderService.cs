using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Ordering.Http.Clients.Interfaces;
using Business.Ordering.Http.Services.Interfaces;



namespace Business.Ordering.Http.Services
{
    public class HttpOrderService : IHttpOrderService
    {
        private readonly IHttpOrderClient _httpOrderClient;
        private readonly IServiceResultFactory _resultFact;

        public HttpOrderService(IHttpOrderClient httpOrderClient, IServiceResultFactory resultFact)
        {
            _httpOrderClient = httpOrderClient;
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<OrderReadDTO>> CompleteOrder(int userId)
        {
            var response = await _httpOrderClient.CompleteOrder(userId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<OrderReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<OrderReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<OrderReadDTO>> CreateOrder(int userId, OrderCreateDTO orderCreateDTO)
        {
            var response = await _httpOrderClient.CreateOrder(userId, orderCreateDTO);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<OrderReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<OrderReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<OrderReadDTO>> DeleteOrder(int userId)
        {
            var response = await _httpOrderClient.DeleteOrder(userId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<OrderReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<OrderReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<OrderReadDTO>>> GetAllOrders()
        {
            var response = await _httpOrderClient.GetAllOrders();

            //if (!response.IsSuccessStatusCode)
            //    return _resultFact.Result<IEnumerable<OrderReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<OrderReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByCartId(Guid cartId)
        {
            var response = await _httpOrderClient.GetOrderByCartId(cartId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<OrderReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<OrderReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByOrderCode(string code)
        {
            var response = await _httpOrderClient.GetOrderByOrderCode(code);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<OrderReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<OrderReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<OrderReadDTO>> GetOrderByUserId(int userId)
        {
            var response = await _httpOrderClient.GetOrderByUserId(userId);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<OrderReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<OrderReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<OrderReadDTO>> UpdateOrder(int userId, OrderUpdateDTO orderUpdateDTO)
        {
            var response = await _httpOrderClient.UpdateOrder(userId, orderUpdateDTO);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<OrderReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<OrderReadDTO>>(content);

            return result;
        }
    }
}
