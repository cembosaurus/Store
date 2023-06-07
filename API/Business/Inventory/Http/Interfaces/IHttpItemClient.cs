using Business.Inventory.DTOs.CatalogueItem;
using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Inventory.Http.Interfaces
{
    public interface IHttpItemClient
    {
        Task<HttpResponseMessage> AddItem(ItemCreateDTO itemDTO);
        Task<HttpResponseMessage> DeleteItem(int itemId);
        Task<HttpResponseMessage> GetItemById(int itemId);
        Task<HttpResponseMessage> GetItems(IEnumerable<int> itemIds = null);
        Task<HttpResponseMessage> UpdateItem(int itemId, ItemUpdateDTO itemDTO);
    }
}
