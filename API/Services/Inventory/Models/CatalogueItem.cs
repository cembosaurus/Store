namespace Inventory.Models
{
    public class CatalogueItem
    {
        public int ItemId { get; set; }
        public string? Description { get; set; }
        public int Instock { get; set; }

        public ItemPrice ItemPrice { get; set; }
        public Item Item { get; set; }

        public ICollection<AccessoryItem> Accessories { get; set; }
        public ICollection<SimilarProductItem> SimilarProducts { get; set; }
    }
}
