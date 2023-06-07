namespace Inventory.Consumer.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? PhotoURL { get; set; }
        public bool Archived { get; set; }
    }
}
