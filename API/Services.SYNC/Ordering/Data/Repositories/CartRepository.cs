using Business.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Data.Repositories.Interfaces;
using Services.Ordering.Models;
using System.Linq;

namespace Ordering.Data.Repositories
{
    public class CartRepository : BaseRepository<OrderingContext>, ICartRepository
    {

        private readonly OrderingContext _context;


        public CartRepository(OrderingContext context) : base(context)
        {
            _context = context;
        }



        public override int SaveChanges()
        {
            return _context.SaveChanges();
        }




        public async Task<IEnumerable<Cart>> GetCards()
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId == c.UserId).ToListAsync();
        }


        public async Task<Guid> GetCartIdByUserId(int userId)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId == c.UserId).Where(c => c.UserId == userId).Select(c => c.CartId).FirstOrDefaultAsync();
        }


        public async Task<int> GetUserIdByCartId(Guid cartId)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId == c.UserId).Where(c => c.CartId == cartId).Select(c => c.UserId).FirstOrDefaultAsync();
        }


        public async Task<Cart> GetCartByUserId(int userId)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId == c.UserId).Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        }


        public async Task<Cart> GetCartWithOrderByUserId(int userId)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId == c.UserId).Include(c => c.CartItems).Include(c => c.Order).Include(c => c.Order.OrderDetails).FirstOrDefaultAsync(c => c.UserId == userId);
        }


        public async Task<Cart> GetCartByCartId(Guid cartId)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId == c.UserId).Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == cartId);
        }


        public async Task<EntityEntry> CreateCart(Cart cart)
        {
            return await _context.Carts.AddAsync(cart);
        }


        public async Task<ActiveCart> GetActiveCart(Guid cartId)
        {
            return await _context.ActiveCarts.SingleOrDefaultAsync(ac => ac.CartId == cartId);
        }


        public async Task<IEnumerable<ActiveCart>> GetActiveCarts()
        {
            return await _context.ActiveCarts.ToListAsync();
        }


        public async Task<EntityEntry> CreateActiveCart(ActiveCart activeCart)
        {
            return await _context.ActiveCarts.AddAsync(activeCart);
        }


        public async Task<EntityEntry> DeleteActiveCart(ActiveCart activeCart)
        {
            return _context.ActiveCarts.Remove(activeCart);
        }


        public async Task<IEnumerable<CartItem>> GetCartItems(Guid cartId)
        {
            return await _context.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();
        }


        public async Task<EntityEntry> DeleteCart(Cart cart)
        {
            return _context.Carts.Remove(cart);
        }


        public async Task<bool> ExistsByUserId(int id)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId == c.UserId).AnyAsync(c => c.UserId == id);
        }


        public async Task<bool> ExistsByCartId(Guid id)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId == c.UserId).AnyAsync(c => c.CartId == id);
        }

    }
}
