using Business.Libraries.ServiceResult.Interfaces;
using Services.Inventory.Models;

namespace Inventory.Services.Tools.Interfaces
{
    public interface IServiceTools
    {
        IServiceResult<CatalogueItem> AddExtrasToCatalogItem(CatalogueItem catalogueItem, IEnumerable<int>? Accessories, IEnumerable<int>? SimilarProducts);
        IServiceResult<CatalogueItem> RemoveExtrasFromCatalogItem(CatalogueItem catalogueItem, IEnumerable<int> Accessories, IEnumerable<int>? SimilarProducts);
    }
}
