using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;
using Services.Ordering.Models;

namespace Ordering.OrderingBusinessLogic.Interfaces
{
    public interface ICartBusinessLogic
    {
        Task<IServiceResult<IEnumerable<CartItem>>> AddItemsToCart(Cart cart, IEnumerable<CartItem> source);
        Task<IServiceResult<IEnumerable<CartItem>>> RemoveItemsFromCart(Cart cart, IEnumerable<CartItem> source);
        Task<IServiceResult<double>> UpdateCartTotal(Cart cart);
        Task<IServiceResult<IEnumerable<int>>> CartItemsLock(Guid cartId, IEnumerable<int> itemsIds);
        Task<IServiceResult<IEnumerable<int>>> CartItemsUnLock(Guid cartId, IEnumerable<int> itemsIds);
        Task<IServiceResult<int>> RemoveAmountFromStock(int itemId, int amount);        
        Task<IServiceResult<int>> AddAmountToStock(int itemId, int amount);

        //IServiceResult<string> CreateOrderCode(int cartId);
        //IServiceResult<string> DecodeOrderCode(string orderId);

    }
}
