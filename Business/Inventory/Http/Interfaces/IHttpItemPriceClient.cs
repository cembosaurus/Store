using Business.Inventory.DTOs.ItemPrice;
using Business.Libraries.ServiceResult.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Inventory.Http.Interfaces
{
    public interface IHttpItemPriceClient
    {
        Task<HttpResponseMessage> GetItemPriceById(int itemId);
        Task<HttpResponseMessage> GetItemPrices(IEnumerable<int> itemIds);
        Task<HttpResponseMessage> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceUpdateDTO);
    }
}
