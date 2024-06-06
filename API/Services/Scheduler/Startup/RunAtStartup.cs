using Business.Identity.Http.Services.Interfaces;
using Scheduler.Tasks.Interfaces;

namespace Scheduler.Startup
{
    public class RunAtStartup : IRunAtStartup
    {

        private readonly IHttpApiKeyAuthService _httpApiKeyAuthService;
        private readonly ICartItemLocker _cartItemLocker;

        private bool isAuthenticated = false;


        public RunAtStartup(IHttpApiKeyAuthService httpApiKeyAuthService, ICartItemLocker cartItemLocker)
        {
            _httpApiKeyAuthService = httpApiKeyAuthService;
            _cartItemLocker = cartItemLocker;
        }



        public async Task Run()
        {
            await Authenticate();

            // Call the apps that require authentication:
            if (isAuthenticated)
            {

                await RemoveExpiredItemsFromCart();

            }

            // Call the apps that don't require authentication:

        }



        // Calls:

        private async Task Authenticate()
        {
            try
            {
                await _httpApiKeyAuthService.Authenticate();

                isAuthenticated = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> APP STARTUP: - FAILED to authenticate Scheduler service ! Reason: '{ex.Message}'");     // To Do: add ex catching middleware 
            }
        }


        private async Task RemoveExpiredItemsFromCart()
        {
            try
            {
                await _cartItemLocker.ExecuteManualy();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> APP STARTUP: - FAILED to run 'Cart Item Locker' task ! Reason: '{ex.Message}'");
            }
        }
    }
}
