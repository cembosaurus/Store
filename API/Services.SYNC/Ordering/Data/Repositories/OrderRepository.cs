using Business.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Data.Repositories.Interfaces;
using Services.Ordering.Models;

namespace Ordering.Data.Repositories
{
    public class OrderRepository : BaseRepository<OrderingContext>, IOrderRepository
    {

        private readonly OrderingContext _context;



        public OrderRepository(OrderingContext context) : base(context)
        {
            _context = context;
        }



        public override int SaveChanges()
        {
            return _context.SaveChanges();
        }




        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.Where(o => o.Cart.ActiveCart.CartId == o.CartId).Include(o => o.OrderDetails).ToListAsync();
        }



        public async Task<Order> GetOrderByUserId(int userId)
        {
            return await _context.Orders.Where(o => o.Cart.ActiveCart.CartId == o.CartId).FirstOrDefaultAsync(o => o.Cart.UserId == userId);
        }


        public async Task<Order> GetOrderWithDetailsByUserId(int userId)
        {
            return await _context.Orders.Where(o => o.Cart.ActiveCart.CartId == o.CartId).Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Cart.UserId == userId);
        }


        public async Task<Order> GetOrderByCartId(Guid cardId)
        {
            return await _context.Orders.Where(o => o.Cart.ActiveCart.CartId == o.CartId).FirstOrDefaultAsync(o => o.CartId == cardId);
        }


        public async Task<Order> GetOrderByOrderCode(string code)
        {
            return await _context.Orders.Where(o => o.Cart.ActiveCart.CartId == o.CartId).FirstOrDefaultAsync(o => o.OrderCode == code);
        }


        public async Task<IEnumerable<OrderDetails>> GetAllOrderDetails()
        {
            return await _context.OrderDetails.Where(o => o.Order.Cart.ActiveCart.CartId == o.CartId).ToListAsync();
        }



        public async Task<OrderDetails> GetOrderDetailsByCartId(Guid cartId)
        {
            return await _context.OrderDetails.Where(o => o.Order.Cart.ActiveCart.CartId == o.CartId).FirstOrDefaultAsync(od => od.CartId == cartId);
        }



        public async Task<EntityEntry> CreateOrder(Order order)
        {
            return await _context.Orders.AddAsync(order);
        }


        public async Task<EntityEntry> CreateOrderDetails(OrderDetails orderDetails)
        {
            return await _context.OrderDetails.AddAsync(orderDetails);
        }


        public async Task<EntityEntry> DeleteOrder(Order order)
        {
            return _context.Orders.Remove(order);
        }


        public async Task<EntityEntry> DeleteOrderDetails(OrderDetails orderDetails)
        {
            return _context.OrderDetails.Remove(orderDetails);
        }


        public async Task<bool> ExistsOrderByCartId(Guid cartId)
        {
            return await _context.Orders.Where(o => o.Cart.ActiveCart.CartId == o.CartId).Where(o => o.Cart.ActiveCart.CartId == o.CartId).AnyAsync(o => o.CartId == cartId);
        }


        public async Task<bool> ExistsOrderDetailsByCartId(Guid cartId)
        {
            return await _context.OrderDetails.Where(o => o.Order.Cart.ActiveCart.CartId == o.CartId).AnyAsync(o => o.CartId == cartId);
        }


    }
}
