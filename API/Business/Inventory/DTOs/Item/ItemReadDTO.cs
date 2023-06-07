namespace Business.Inventory.DTOs.Item
{
    public class ItemReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? PhotoURL { get; set; }
    }
}
