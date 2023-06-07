using Business.Libraries.ServiceResult.Interfaces;

namespace Scheduler.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<IServiceResult<string>> AuthenticateService();
    }
}
