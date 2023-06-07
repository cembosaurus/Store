namespace Inventory.Models
{
    public class SimilarProductItem
    {
        public int ItemId { get; set; }
        public int SimilarProductItemId { get; set; }

        public virtual CatalogueItem CatalogueItem { get; set; }
    }
}
