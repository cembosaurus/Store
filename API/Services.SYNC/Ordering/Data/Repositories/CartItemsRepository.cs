using Business.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Data.Repositories.Interfaces;
using Services.Ordering.Models;

namespace Ordering.Data.Repositories
{
    public class CartItemsRepository : BaseRepository<OrderingContext>, ICartItemsRepository
    {

        private readonly OrderingContext _context;


        public CartItemsRepository(OrderingContext context) : base(context)
        {
            _context = context;
        }




        public override int SaveChanges()
        {
            return _context.SaveChanges();
        }





        public async Task<IEnumerable<CartItem>> GetAllCardItems()
        {
            return await _context.Carts.Where(c => c.UserId == c.ActiveCart.UserId)
                .Select(c => c.CartItems.Where(ci => ci.CartId == c.CartId))
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<CartItem>> GetCartItemsByUserId(int userId)
        {
            return await _context.Carts.Where(c => c.UserId == userId && c.UserId == c.ActiveCart.UserId)
                .Select(c => c.CartItems.Where(ci => ci.CartId == c.CartId))
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<CartItem>> GetCartItemsByCartId(Guid cartId)
        {
            return await _context.Carts.Where(c => c.CartId == cartId && c.UserId == c.ActiveCart.UserId)
                .Select(c => c.CartItems.Where(ci => ci.CartId == c.CartId))
                .FirstOrDefaultAsync();
        }



        public async Task<EntityEntry> DeleteCartItem(CartItem cartItem)
        {
            return _context.CartItems.Remove(cartItem);
        }


        public async Task DeleteCartItems(IEnumerable<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
        }

    }
}
