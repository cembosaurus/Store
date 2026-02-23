namespace Business.Inventory.DTOs.ItemPrice
{
    public class ItemPriceReadDTO
    {
        public int ItemId { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? RRP { get; set; }
        public int? DiscountPercent { get; set; }
    }
}
