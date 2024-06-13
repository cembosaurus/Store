using Scheduler.Tasks.Interfaces;



namespace Scheduler.Startup
{
    public class RunAtStartup : IRunAtStartup
    {

        private readonly ICartItemLocker _cartItemLocker;


        public RunAtStartup(ICartItemLocker cartItemLocker)
        {
            _cartItemLocker = cartItemLocker;
        }



        public async Task Run()
        {
                await RemoveExpiredItemsFromCart();
        }




        private async Task RemoveExpiredItemsFromCart()
        {
            try
            {
                await _cartItemLocker.RemoveExpiredLocks();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> APP STARTUP: - FAILED to remove expired cart-item locks ! Reason: '{ex.Message}'");
            }
        }
    }
}
