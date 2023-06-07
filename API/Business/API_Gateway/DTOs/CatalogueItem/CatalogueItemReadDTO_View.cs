namespace Business.API_Gateway.DTOs.CatalogueItem
{
    public class CatalogueItemReadDTO_View
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double SalesPrice { get; set; }
        public int Amount { get; set; }
    }
}
