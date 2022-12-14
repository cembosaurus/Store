namespace Business.Scheduler.DTOs
{
    public class CartItemsLockReadDTO
    {
        public IEnumerable<int> ItemsIds { get; set; }
        public Guid CartId { get; set; }
        public int Amount { get; set; }
        public int LockedForDays { get; set; }
        public DateTime Locked { get; set; }
    }
}
