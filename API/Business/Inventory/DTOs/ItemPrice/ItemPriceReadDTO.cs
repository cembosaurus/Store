namespace Business.Inventory.DTOs.ItemPrice
{
    public class ItemPriceReadDTO
    {
        public int ItemId { get; set; }
        public double SalePrice { get; set; }
        public double? RRP { get; set; }
        public int? DiscountPercent { get; set; }
    }
}
