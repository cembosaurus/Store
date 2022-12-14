using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Inventory.DTOs.CatalogueItem.SimilarProductItem
{
    public class SimilarProductItemReadDTO
    {
        public int ItemId { get; set; }
        public int SimilarProductItemId { get; set; }
    }
}
