using Business.Libraries.ServiceResult.Interfaces;



namespace Business.Identity.Http.Services.Interfaces
{
    public interface IHttpApiKeyAuthService
    {
        Task<IServiceResult<string>> Authenticate();
    }
}
