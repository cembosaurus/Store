using API_Gateway.Services.Business.Ordering.Interfaces;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;



namespace API_Gateway.Services.Business.Ordering
{
    public class CartService : ICartService
    {

        private readonly IHttpCartService _httpCartService;

        public CartService(IHttpCartService httpCartService)
        {
            _httpCartService = httpCartService;
        }




        public async Task<IServiceResult<IEnumerable<CartReadDTO>>> GetCards()
        {
            return await _httpCartService.GetCards();
        }



        public async Task<IServiceResult<CartReadDTO>> GetCartByUserId(int userId)
        {
            return await _httpCartService.GetCartByUserId(userId);
        }



        public async Task<IServiceResult<CartReadDTO>> CreateCart(int userId)
        {
            return await _httpCartService.CreateCart(userId);
        }



        public async Task<IServiceResult<CartReadDTO>> UpdateCart(int userId, CartUpdateDTO cartUpdateDTO)
        {
            return await _httpCartService.UpdateCart(userId, cartUpdateDTO);
        }



        public async Task<IServiceResult<CartReadDTO>> DeleteCart(int userId)
        {
            return await _httpCartService.DeleteCart(userId);
        }




        public async Task<IServiceResult<bool>> ExistsCartByCartId(Guid cartId)
        {
            return await _httpCartService.ExistsCartByCartId(cartId);
        }


    }
}
