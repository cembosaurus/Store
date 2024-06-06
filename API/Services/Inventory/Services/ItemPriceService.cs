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
            var itemPrices = await _repo.GetItemPrices(itemIds);

            if (itemPrices == null || !itemPrices.Any())
                return _resultFact.Result<IEnumerable<ItemPriceReadDTO>>(null, true, "NO item prices found !");

            return _resultFact.Result(
                _mapper.Map<IEnumerable<ItemPriceReadDTO>>(itemPrices), 
                true, 
                $"{(itemIds == null ? "" : (itemIds.Count() > itemPrices.Count() ? $"Prices for {itemIds.Count() - itemPrices.Count()} items were not found ! Reason: Items may not be registered in catalogue." : ""))}");
        }



        public async Task<IServiceResult<ItemPriceReadDTO>> GetItemPriceById(int id)
        {
            var message = "";
            var itemPrice = await _repo.GetItemPriceById(id);

            if (itemPrice == null)
            {
                message = $"Catalogue item with Id '{id}' NOT found";

                if (await _repo.ItemExistsById(id))
                    message += $", but Item with Id '{id}' EXIST and is NOT labeled with price";

                return _resultFact.Result<ItemPriceReadDTO>(null, false, message + " !");
            }

            return _resultFact.Result(_mapper.Map<ItemPriceReadDTO>(itemPrice), true, message);
        }




        public async Task<IServiceResult<ItemPriceReadDTO>> UpdateItemPrice(int itemId, ItemPriceUpdateDTO itemPriceEditDTO)
        {
            var itemPrice = await _repo.GetItemPriceById(itemId);

            if (itemPrice == null)
                return _resultFact.Result<ItemPriceReadDTO>(null, false, $"Item price '{itemId}': NOT found !");

            _mapper.Map(itemPriceEditDTO, itemPrice);

            if (_repo.SaveChanges() < 1)
                return _resultFact.Result<ItemPriceReadDTO>(null, false, $"Item price '{itemId}': changes were NOT saved into DB !");

            return _resultFact.Result(_mapper.Map<ItemPriceReadDTO>(itemPrice), true);
        }


    }
}
