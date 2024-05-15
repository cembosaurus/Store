using Business.Inventory.DTOs.CatalogueItem;
using Business.Libraries.ServiceResult.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Inventory.Http.Clients.Interfaces
{
    public interface IHttpCatalogueItemClient
    {
        Task<HttpResponseMessage> AddAmountToStock(int itemId, int amount);
        Task<HttpResponseMessage> AddExtrasToCatalogueItem(int itemId, ExtrasAddDTO extrasAddDTO);
        Task<HttpResponseMessage> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItemCreateDTO);
        Task<HttpResponseMessage> ExistsCatalogueItemById(int itemId);
        Task<HttpResponseMessage> GetCatalogueItemById(int itemId);
        Task<HttpResponseMessage> GetCatalogueItems(IEnumerable<int> itemIds = null);
        Task<HttpResponseMessage> GetCatalogueItemWithExtrasById(int itemId);
        Task<HttpResponseMessage> GetInstockCount(int itemId);
        Task<HttpResponseMessage> RemoveCatalogueItem(int itemId);
        Task<HttpResponseMessage> RemoveExtrasFromCatalogueItem(int itemId, ExtrasRemoveDTO extrasRemoveDTO);
        Task<HttpResponseMessage> RemoveFromStockAmount(int itemId, int amount);
        Task<HttpResponseMessage> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO catalogueItemUpdateDTO);
    }
}
