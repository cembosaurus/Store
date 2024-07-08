using Scheduler.StartUp.Interfaces;
using Scheduler.Tasks.Interfaces;



namespace Scheduler.StartUp
{
    public class RunAtStartUp : IRunAtStartUp
    {
        private readonly ICartItemLocker _cartItemLocker;


        public RunAtStartUp(ICartItemLocker cartItemLocker)
        {
            _cartItemLocker = cartItemLocker;
        }



        public async Task Run()
        {
            await RemoveExpiredItemsFromCart();
        }




        public async Task RemoveExpiredItemsFromCart() => await _cartItemLocker.RemoveExpiredLocks();
    }
}
