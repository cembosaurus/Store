using Business.API_Gateway.DTOs.CatalogueItem;

namespace Business.Ordering.DTOs
{
    public class CartReadDTOForUser
    {
        public int UserId { get; set; }
        public double Total { get; set; }
        public IEnumerable<CatalogueItemReadDTO_View> CatalogueItems { get; set; }
    }
}
