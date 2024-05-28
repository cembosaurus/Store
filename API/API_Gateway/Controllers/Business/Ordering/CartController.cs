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
        public async Task<IActionResult> GetCarts()
        {
            var result = await _cartService.GetCards();

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // customer
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var result = await _cartService.GetCartByUserId(_principalId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // management
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUsersCart(int userId)
        {
            var result = await _cartService.GetCartByUserId(userId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]       // manager
        [HttpGet("items/all")]
        public async Task<IActionResult> GetAllCardItems()
        {
            var result = await _cartIItemService.GetAllCardItems();

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // customer
        [HttpGet("items")]
        public async Task<IActionResult> GetCartItems()
        {
            var result = await _cartIItemService.GetCartItems(_principalId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // manager
        [HttpGet("{userId}/items")]
        public async Task<IActionResult> GetUsersCartItems(int userId)
        {
            var result = await _cartIItemService.GetCartItems(userId);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        // POST:

        // Cart:


        [Authorize(Policy = "Everyone")]            // only customer
        [HttpPost]
        public async Task<IActionResult> CreateCart()
        {
            var result = await _cartService.CreateCart(_principalId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]            // customer
        [HttpPost("items")]
        public async Task<IActionResult> AddItemsToCart([FromBody] IEnumerable<CartItemUpdateDTO> items)
        {
            var result = await _cartIItemService.AddItemsToCart(_principalId, items);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpPost("{userId}/items")]
        public async Task<IActionResult> AddItemsToUsersCart(int userId, IEnumerable<CartItemUpdateDTO> items)
        {
            var result = await _cartIItemService.AddItemsToCart(userId, items);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        // PUT:

        // Cart:

        [Authorize(Policy = "Everyone")]        // customer
        [HttpPut]
        public async Task<IActionResult> UpdateCart(CartUpdateDTO cartUpdateDTO)
        {
            var result = await _cartService.UpdateCart(_principalId, cartUpdateDTO);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]        // management
        [HttpPut("{UserId}")]
        public async Task<IActionResult> UpdateUsersCart(int userid, CartUpdateDTO cartUpdateDTO)
        {
            var result = await _cartService.UpdateCart(userid, cartUpdateDTO);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        // DELETE:

        // Cart:


        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete]
        public async Task<IActionResult> DeleteCart()
        {
            var result = await _cartService.DeleteCart(_principalId);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}")]
        public async Task<IActionResult> DeleteUsersCart(int userid)
        {
            var result = await _cartService.DeleteCart(userid);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        // Cart Items:

        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete("items")]
        public async Task<IActionResult> RemoveCartItems(IEnumerable<CartItemUpdateDTO> items)
        {
            var result = await _cartIItemService.RemoveCartItems(_principalId, items);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}/items")]
        public async Task<IActionResult> RemoveUsersCartItems(int userId, IEnumerable<CartItemUpdateDTO> items)
        {
            var result = await _cartIItemService.RemoveCartItems(userId, items);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // customer
        [HttpDelete("items/delete")]
        public async Task<IActionResult> DeleteCartItems(IEnumerable<int> items)
        {
            var result = await _cartIItemService.DeleteCartItems(_principalId, items);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [Authorize(Policy = "Everyone")]            // management
        [HttpDelete("{UserId}/items/delete")]
        public async Task<IActionResult> DeleteUsersCartItems(int userId, IEnumerable<int> items)
        {
            var result = await _cartIItemService.DeleteCartItems(userId, items);

            return result.Status ? Ok(result) : BadRequest(result);
        }



    }
}
