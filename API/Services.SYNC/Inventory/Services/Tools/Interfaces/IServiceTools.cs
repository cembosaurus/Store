using Inventory.Models;
using Business.Libraries.ServiceResult.Interfaces;

namespace Inventory.Services.Tools.Interfaces
{
    public interface IServiceTools
    {
        IServiceResult<CatalogueItem> AddExtrasToCatalogItem(CatalogueItem catalogueItem, IEnumerable<int>? Accessories, IEnumerable<int>? SimilarProducts);
        IServiceResult<CatalogueItem> RemoveExtrasFromCatalogItem(CatalogueItem catalogueItem, IEnumerable<int> Accessories, IEnumerable<int>? SimilarProducts);
    }
}
