namespace Inventory.Models
{
    public class ItemPrice
    {
        public int ItemId { get; set; }        
        public double SalePrice { get; set; }
        public double? RRP { get; set; }
        public int? DiscountPercent { get; set; }

        public CatalogueItem CatalogueItem { get; set; }
    }
}
