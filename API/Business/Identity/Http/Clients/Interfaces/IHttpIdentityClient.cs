using Business.Identity.DTOs;

namespace Business.Identity.Http.Clients.Interfaces
{
    public interface IHttpIdentityClient
    {
        Task<HttpResponseMessage> AuthenticateService(string apiKey);
        Task<HttpResponseMessage> Login(UserToLoginDTO user);
        Task<HttpResponseMessage> Register(UserToRegisterDTO user);

        //Task<IServiceResult<UserReadDTO>> GetCurrentUser();
    }
}
