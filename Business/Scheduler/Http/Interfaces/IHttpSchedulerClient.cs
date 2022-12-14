using Business.Scheduler.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Scheduler.Http.Interfaces
{
    public interface IHttpSchedulerClient
    {
        Task<HttpResponseMessage> CartItemsLock(CartItemsLockCreateDTO cartItemsToLock);
        Task<HttpResponseMessage> CartItemsUnLock(CartItemsLockDeleteDTO cartItemsToUnLock);
    }
}
