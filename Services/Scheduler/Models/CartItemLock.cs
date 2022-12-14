namespace Scheduler.Models
{
    public class CartItemLock
    {
        public int ItemId { get; set; }
        public Guid CartId { get; set; }
        public int LockedForDays { get; set; }
        public DateTime Locked { get; set; }
    }
}
