using Business.Libraries.ServiceResult.Interfaces;

namespace Scheduler.Tasks.Interfaces
{
    public interface ICartItemLocker
    {
        Task<IServiceResult<bool>> ExecuteManualy();
    }
}
