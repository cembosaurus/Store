using AutoMapper;
using Business.Inventory.DTOs.ItemPrice;
using Business.Libraries.ServiceResult.Interfaces;
using Inventory.Services.Interfaces;
using Services.Inventory.Data.Repositories.Interfaces;

namespace Inventory.Services
{
    public class ItemPriceService: IItemPriceService
    {

        private readonly IItemPriceRepository _repo;
        private readonly IMapper _mapper;
        private readonly IServiceResultFactory _resultFact;


        public ItemPriceService(IItemPriceRepository repo, IMapper mapper, IServiceResultFactory resultFact)
        {
            _repo = repo;
            _mapper = mapper;
            _resultFact = resultFact;
        }



        public async Task<IServiceResult<IEnumerable<ItemPriceReadDTO>>> GetItemPrices(IEnumerable<int> itemIds = default)
        {
            Console.WriteLine($"--> GETTING item prices ......");


            var itemPrices = await _repo.GetItemPrices(itemIds);

            if (itemPrices == null || !itemPrices.Any())
                return _resultFact.Result<IEnumerable<ItemPriceReadDTO>>(null, false, "NO item prices found !");

            return _resultFact.Result(
                _mapper.Map<IEnumerable<ItemPriceReadDTO>>(itemPrices), 
                true, 
                $"{(itemIds == null ? "" : (itemIds.Count() > itemPrices.Count() ? $"Prices for {itemIds.Count() - itemPrices.Count()} items were not found ! Reason: Items may not be registered in catalogue." : ""))}");
        }



        public async Task<IServiceResult<ItemPriceReadDTO>> GetItemPriceById(int id)
        {
            Console.WriteLine($"--> GETTING item price '{id}' ......");


            var itemPrice = await _repo.GetItemPriceById(id);

            if (itemPrice == null)
            {
                if (await _repo.ItemExistsById(id))
                    return _resultFact.Result<ItemPriceReadDTO>(null, false, $"Item with Id '{id}' EXIST but was NOT labeled with price !");

                return _resultFact.Result<ItemPriceReadDTO>(null, false, $"Catalogue item with Id '{id}' NOT found !");
            }

            return _resultFact.Result(_mapper.Map<ItemPriceReadDTO>(itemPrice), true);
        }




        public async Task<IServiceResult<ItemPriceReadDTO>> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceEditDTO)
        {
            var itemPrice = await _repo.GetItemPriceById(itemId);

            if (itemPrice == null)
                return _resultFact.Result<ItemPriceReadDTO>(null, false, $"Item price '{itemId}': NOT found !");


            Console.WriteLine($"--> UPDATING item price '{itemPrice.ItemId}' ......");


            _mapper.Map(itemPriceEditDTO, itemPrice);

            if (_repo.SaveChanges() < 1)
                return _resultFact.Result<ItemPriceReadDTO>(null, false, $"Item price '{itemId}': changes were NOT saved into DB !");

            return _resultFact.Result(_mapper.Map<ItemPriceReadDTO>(itemPrice), true);
        }


    }
}
