using API_Gateway.Services.Business.Ordering.Interfaces;
using Business.Ordering.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Gateway.Controllers.Business.Ordering
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly int _principalId;
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartIItemService;

        public CartController(IHttpContextAccessor accessor, ICartService cartService, ICartItemService cartIItemService)
        {
            int.TryParse(accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out _principalId);
            _cartService = cartService;
            _cartIItemService = cartIItemService;
        }






        // GET:

        // Cart:

        [Authorize(Policy = "Everyone")]            // management
        [HttpGet("all")]
        public async Task<object> GetCarts()
        {
            var result = await _cartService.GetCards();

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // customer
        [HttpGet]
        public async Task<object> GetCart()
        {
            var result = await _cartService.GetCartByUserId(_principalId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // management
        [HttpGet("{userId}")]
        public async Task<object> GetUsersCart(int userId)
        {
            var result = await _cartService.GetCartByUserId(userId);

            return result;  // ctr res
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]       // manager
        [HttpGet("items/all")]
        public async Task<object> GetAllCardItems()
        {
            var result = await _cartIItemService.GetAllCardItems();

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // customer
        [HttpGet("items")]
        public async Task<object> GetCartItems()
        {
            var result = await _cartIItemService.GetCartItems(_principalId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // manager
        [HttpGet("{userId}/items")]
        public async Task<object> GetUsersCartItems(int userId)
        {
            var result = await _cartIItemService.GetCartItems(userId);

            return result;  // ctr res
        }




        // POST:

        // Cart:


        [Authorize(Policy = "Everyone")]            // only customer
        [HttpPost]
        public async Task<object> CreateCart()
        {
            var result = await _cartService.CreateCart(_principalId);

            return result;  // ctr res
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]            // customer
        [HttpPost("items")]
        public async Task<object> AddItemsToCart([FromBody] IEnumerable<CartItemUpdateDTO> items)
        {
            var result = await _cartIItemService.AddItemsToCart(_principalId, items);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpPost("{userId}/items")]
        public async Task<object> AddItemsToUsersCart(int userId, IEnumerable<CartItemUpdateDTO> items)
        {
            var result = await _cartIItemService.AddItemsToCart(userId, items);

            return result;  // ctr res
        }




        // PUT:

        // Cart:

        [Authorize(Policy = "Everyone")]        // customer
        [HttpPut]
        public async Task<object> UpdateCart(CartUpdateDTO cartUpdateDTO)
        {
            var result = await _cartService.UpdateCart(_principalId, cartUpdateDTO);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]        // management
        [HttpPut("{UserId}")]
        public async Task<object> UpdateUsersCart(int userid, CartUpdateDTO cartUpdateDTO)
        {
            var result = await _cartService.UpdateCart(userid, cartUpdateDTO);

            return result;  // ctr res
        }




        // DELETE:

        // Cart:


        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete]
        public async Task<object> DeleteCart()
        {
            var result = await _cartService.DeleteCart(_principalId);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}")]
        public async Task<object> DeleteUsersCart(int userid)
        {
            var result = await _cartService.DeleteCart(userid);

            return result;  // ctr res
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete("items")]
        public async Task<object> RemoveCartItems(IEnumerable<CartItemUpdateDTO> items)
        {
            var result = await _cartIItemService.RemoveCartItems(_principalId, items);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}/items")]
        public async Task<object> RemoveUsersCartItems(int userId, IEnumerable<CartItemUpdateDTO> items)
        {
            var result = await _cartIItemService.RemoveCartItems(userId, items);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete("items/delete")]
        public async Task<object> DeleteCartItems(IEnumerable<int> items)
        {
            var result = await _cartIItemService.DeleteCartItems(_principalId, items);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}/items/delete")]
        public async Task<object> DeleteUsersCartItems(int userId, IEnumerable<int> items)
        {
            var result = await _cartIItemService.DeleteCartItems(userId, items);

            return result;  // ctr res
        }



    }
}
