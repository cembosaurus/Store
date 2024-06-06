using AutoMapper;
using Business.Inventory.DTOs.Item;
using Inventory.Models;
using Business.Libraries.ServiceResult.Interfaces;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Inventory.Data.Repositories.Interfaces;



namespace Inventory.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repo;
        private readonly IMapper _mapper;
        private readonly IServiceResultFactory _resultFact;

        public ItemService(IItemRepository repo, IMapper mapper, IServiceResultFactory resultFact)
        {
            _repo = repo;
            _mapper = mapper;
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<IEnumerable<ItemReadDTO>>> GetItems(IEnumerable<int> itemIds = null)
        {
            var items = await _repo.GetItems(itemIds);

            return _resultFact.Result(_mapper.Map<IEnumerable<ItemReadDTO>>(items), true, !items.Any() ? "NO items found !" : "");
        }



        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int id)
        {
            var item = await _repo.GetItemById(id);

            return _resultFact.Result(_mapper.Map<ItemReadDTO>(item), true, item == null ? $"Item '{id}' was NOT found !" : "");
        }



        public async Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO itemCreateDTO)
        {
            if (await _repo.ExistsByName(itemCreateDTO.Name))
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{itemCreateDTO.Name}' already EXISTS !");

            var item = _mapper.Map<Item>(itemCreateDTO);

            var resultState = await _repo.AddItem(item);


            if (resultState != EntityState.Added || _repo.SaveChanges() < 1)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{itemCreateDTO.Name}' was NOT created");

            return _resultFact.Result(_mapper.Map<ItemReadDTO>(item), true);
        }



        public async Task<IServiceResult<ItemReadDTO>> UpdateItem(int id, ItemUpdateDTO itemUpdateDTO)
        {
            var item = await _repo.GetItemById(id);

            if (item == null)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{id}' NOT found !");
            if(await _repo.ExistsByName(itemUpdateDTO.Name))
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item with name: '{itemUpdateDTO.Name}' already exists !");

            _mapper.Map(itemUpdateDTO, item);

            if (_repo.SaveChanges() < 1)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{id}': changes were NOT saved into DB !");

            return _resultFact.Result(_mapper.Map<ItemReadDTO>(item), true);
        }



        public async Task<IServiceResult<ItemReadDTO>> DeleteItem(int id)
        {
            var item = await _repo.GetItemById(id);

            if (item == null)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{id}' NOT found !");

            if(item.Archived)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{id}' can NOT be deleted because it is ARCHIVED !");

            var resultState = await _repo.DeleteItem(item);

            if (resultState != EntityState.Deleted || _repo.SaveChanges() < 1)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item with id '{id}' was NOT removed from DB !");

            return _resultFact.Result(_mapper.Map<ItemReadDTO>(item), true);
        }

    }
}
