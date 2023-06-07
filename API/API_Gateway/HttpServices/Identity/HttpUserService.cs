using API_Gateway.HttpServices.Identity.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Http.Clients.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;

namespace API_Gateway.HttpServices.Identity
{
    public class HttpUserService : IHttpUserService
    {

        private readonly IHttpUserClient _httpUserClient;
        private readonly IServiceResultFactory _resultFact;

        public HttpUserService(IHttpUserClient httpUserClient, IServiceResultFactory resultFact)
        {
            _httpUserClient = httpUserClient;
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<IEnumerable<string>>> EditUserRoles(int id, IEnumerable<string> roles)
        {
            var response = await _httpUserClient.EditUserRoles(id, roles);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<string>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<string>>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<UserReadDTO>>> GetAllUsers()
        {
            var response = await _httpUserClient.GetAllUsers();

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<UserReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<UserReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<IEnumerable<UserWithRolesReadDTO>>> GetAllUsersWithRoles()
        {
            var response = await _httpUserClient.GetAllUsersWithRoles();

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<UserWithRolesReadDTO>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<UserWithRolesReadDTO>>>(content);

            return result;
        }



        public async Task<IServiceResult<UserReadDTO>> GetCurrentUser()
        {
            var response = await _httpUserClient.GetCurrentUser();

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<UserReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<UserReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<UserReadDTO>> GetUserById(int id)
        {
            var response = await _httpUserClient.GetUserById(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<UserReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<UserReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<UserReadDTO>> GetUserByName(string name)
        {
            var response = await _httpUserClient.GetUserByName(name);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<UserReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<UserReadDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<UserWithRolesReadDTO>> GetUserWithRoles(int id)
        {
            var response = await _httpUserClient.GetUserWithRoles(id);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<UserWithRolesReadDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<UserWithRolesReadDTO>>(content);

            return result;
        }
    }
}
