using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Inventory.DTOs.CatalogueItem
{
    public class ExtrasReadDTO
    {
        public IEnumerable<int>? Accessories { get; set; }
        public IEnumerable<int>? SimilarProducts { get; set; }
    }
}
