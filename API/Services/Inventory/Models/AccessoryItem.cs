namespace Inventory.Models
{
    public class AccessoryItem
    {
        public int ItemId { get; set; }
        public int AccessoryItemId { get; set; }

        public CatalogueItem CatalogueItem { get; set; }
    }
}
