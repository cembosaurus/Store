using Business.Inventory.DTOs.Item;
using Business.Inventory.DTOs.ItemPrice;

namespace Business.Inventory.DTOs.CatalogueItem
{
    public class CatalogueItemReadDTOWithExtras
    {
        public int ItemId { get; set; }
        public string? Description { get; set; }
        public ItemReadDTO Item { get; set; }
        public ItemPriceReadDTO ItemPrice { get; set; }
        public int Instock { get; set; }
        public IEnumerable<CatalogueItemReadDTO>? AccessoryItems { get; set; }
        public IEnumerable<CatalogueItemReadDTO>? SimilarProductItems { get; set; }
    }
}
