using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult.Interfaces;

namespace Inventory.Consumer.Services.Interfaces
{
    public interface IItemService
    {
        Task<IServiceResult<object>> Request(string reqMethod, string reqParam);
    }
}
