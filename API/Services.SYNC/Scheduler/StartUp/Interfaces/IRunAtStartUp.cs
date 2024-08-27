namespace Scheduler.StartUp.Interfaces
{
    public interface IRunAtStartUp
    {
        Task RemoveExpiredItemsFromCart();
        Task Run();
    }
}
