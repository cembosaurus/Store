using Business.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Data.Repositories.Interfaces;
using Services.Ordering.Models;

namespace Ordering.Data.Repositories
{
    public class ArchiveRepository : BaseRepository<OrderingContext>, IArchiveRepository
    {

        private readonly OrderingContext _context;



        public ArchiveRepository(OrderingContext context) : base(context)
        {
            _context = context;
        }



        public override int SaveChanges()
        {
            return _context.SaveChanges();
        }




        // Order:

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.Where(o => o.Cart.ActiveCart.CartId != o.CartId).Include(o => o.OrderDetails).ToListAsync();
        }


        public async Task<IEnumerable<Order>> GetOrdersByUserId(int userId)
        {
            return await _context.Orders.Where(o => o.Cart.ActiveCart.CartId != o.CartId && o.Cart.UserId == userId).Include(o => o.OrderDetails).ToListAsync();
        }


        public async Task<Order> GetOrderByCartId(Guid cardId)
        {
            return await _context.Orders.Where(o => o.Cart.ActiveCart.CartId != o.CartId).Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.CartId == cardId);
        }


        public async Task<Order> GetOrderByOrderCode(string code)
        {
            var test = await _context.Orders.Where(o => o.Cart.ActiveCart.CartId != o.CartId).Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderCode == code);
            return test;
        }


        public async Task DeleteOrdersPermanently(IEnumerable<Order> orders)
        {
            _context.Orders.RemoveRange(orders);
        }




        // Cart:

        public async Task<IEnumerable<Cart>> GetCartsByUserId(int userId)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId != c.UserId && c.UserId == userId).Include(c => c.CartItems).ToListAsync();
        }

        public async Task<Guid> GetCartIdByUserId(int userId)
        {
            return await _context.Carts.Where(c => c.UserId == userId && c.ActiveCart.UserId != c.UserId).Select(c => c.CartId).FirstOrDefaultAsync();
        }


        public async Task<Cart> GetCartByUserId(int userId)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId != c.UserId).Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        }


        public async Task<Cart> GetCartWithOrderByUserId(int userId)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId != c.UserId).Include(c => c.CartItems).Include(c => c.Order).Include(c => c.Order.OrderDetails).FirstOrDefaultAsync(c => c.UserId == userId);
        }


        public async Task<Cart> GetCartById(Guid cartId)
        {
            return await _context.Carts.Where(c => c.ActiveCart.UserId != c.UserId).Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == cartId);
        }

        public async Task DeleteCartsPermanently(IEnumerable<Cart> carts)
        {
            _context.Carts.RemoveRange(carts);
        }
    }
}
