namespace Inventory.Models
{
    public class ItemPrice
    {
        public int ItemId { get; set; }        
        public decimal SalePrice { get; set; }
        public decimal? RRP { get; set; }
        public int? DiscountPercent { get; set; }

        public CatalogueItem CatalogueItem { get; set; } = null!;
    }
}
