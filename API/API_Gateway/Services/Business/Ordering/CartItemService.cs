using API_Gateway.HttpServices.Ordering.Interfaces;
using API_Gateway.Services.Business.Ordering.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Scheduler.DTOs;

namespace API_Gateway.Services.Business.Ordering
{
    public class CartItemService : ICartItemService
    {

        private readonly IHttpCartItemService _httpCartItemService;


        public CartItemService(IHttpCartItemService httpCartItemService)
        {
            _httpCartItemService = httpCartItemService;
        }




        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetAllCardItems()
        {
            return await _httpCartItemService.GetAllCardItems();
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetCartItems(int userId)
        {
            return await _httpCartItemService.GetCartItems(userId);
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> AddItemsToCart(int userId, IEnumerable<CartItemUpdateDTO> itemsToAdd)
        {
            return await _httpCartItemService.AddItemsToCart(userId, itemsToAdd);
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> RemoveCartItems(int userId, IEnumerable<CartItemUpdateDTO> itemsToRemove)
        {
            return await _httpCartItemService.RemoveCartItems(userId, itemsToRemove);
        }

        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> DeleteCartItems(int userId, IEnumerable<int> itemIds)
        {
            return await _httpCartItemService.DeleteCartItems(userId, itemIds);
        }
    }
}
