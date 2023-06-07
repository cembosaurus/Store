using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;
using Quartz;
using Scheduler.Data.Repositories.Interfaces;
using Scheduler.Services.Interfaces;
using Scheduler.Tasks.Interfaces;

namespace Scheduler.Tasks
{
    public class CartItemLocker : IJob, ICartItemLocker
    {
        private readonly ICartItemsService _cartItemService;
        private readonly ICartItemLockRepository _cartItemLockRepo;
        private readonly IServiceResultFactory _resultFact;

        public CartItemLocker(ICartItemLockRepository cartItemLockRepo, IServiceResultFactory resultFact, ICartItemsService cartItemService)
        {
            _cartItemService = cartItemService;
            _cartItemLockRepo = cartItemLockRepo;
            _resultFact = resultFact;
        }





        public async Task Execute(IJobExecutionContext context)
        {
            await ProcessExpiredCartItemLocks();

            Console.WriteLine($"This job will be executed again at: {context.NextFireTimeUtc}");

            return;
        }


        public async Task<IServiceResult<bool>> ExecuteManualy() => await ProcessExpiredCartItemLocks();


        private async Task<IServiceResult<bool>> ProcessExpiredCartItemLocks()
        {
            Console.WriteLine($"--> REMOVING expired items from cart ....");


            var cartItemLocksExpired = await _cartItemLockRepo.GetCartItemLocksExpired();

            if (cartItemLocksExpired == null)
                return _resultFact.Result(false, false, $"Failed to retrieve cart item locks from DB !");

            if (!cartItemLocksExpired.Any())
                return _resultFact.Result(false, true, $"NO expired cart items found to be deleted.");


            var cartsIds = cartItemLocksExpired.Select(c => c.CartId).Distinct().ToList();

            var cartItemsToRemove = new List<CartItemsLockDeleteDTO>();

            foreach (var ci in cartsIds)
            {
                cartItemsToRemove.Add(new CartItemsLockDeleteDTO { 
                    CartId = ci, 
                    ItemsIds = cartItemLocksExpired
                    .Where(w => w.CartId == ci)
                    .Select(cile => cile.ItemId)
                    .ToList()
                });
            }


            var removeResult = await _cartItemService.RemoveExpiredItemsFromCart(cartItemsToRemove);

            if (!removeResult.Status)
                return _resultFact.Result(false, false, $"Expired cart items were NOT removed from carts ! Reason: '{removeResult.Message}'");

            return _resultFact.Result(true, true);
        }
    }
}
