using Business.Libraries.ServiceResult.Interfaces;
using Quartz;

namespace Scheduler.Tasks.Interfaces
{
    public interface ICartItemLocker
    {
        Task Execute(IJobExecutionContext context);
        Task<IServiceResult<bool>> RemoveExpiredLocks();
    }
}
