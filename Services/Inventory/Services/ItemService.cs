using AutoMapper;
using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult.Interfaces;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Inventory.Data.Repositories.Interfaces;
using Services.Inventory.Models;

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

            if (!items.Any())
                return _resultFact.Result<IEnumerable<ItemReadDTO>>(null, false, "NO items found !");

            Console.WriteLine($"--> GETTING items ......");

            return _resultFact.Result(_mapper.Map<IEnumerable<ItemReadDTO>>(items), true);
        }



        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int id)
        {
            var item = await _repo.GetItemById(id);

            if (item == null)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{id}' was NOT found !");


            Console.WriteLine($"--> GETTING item '{item.Id}': '{item.Name}' ......");

            return _resultFact.Result(_mapper.Map<ItemReadDTO>(item), true);
        }



        public async Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO itemCreateDTO)
        {
            if (await _repo.ExistsByName(itemCreateDTO.Name))
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{itemCreateDTO.Name}' already EXISTS !");


            var result = await _repo.AddItem(_mapper.Map<Item>(itemCreateDTO));

            var item = (Item)result.Entity;


            if (result.State != EntityState.Added || _repo.SaveChanges() < 1)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{itemCreateDTO.Name}' was NOT created");

            Console.WriteLine($"--> ADDING item '{item.Id}': '{item.Name}'......");

            return _resultFact.Result(_mapper.Map<ItemReadDTO>(item), true);
        }



        public async Task<IServiceResult<ItemReadDTO>> UpdateItem(int id, ItemUpdateDTO itemUpdateDTO)
        {
            var item = await _repo.GetItemById(id);

            if (item == null)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{id}' NOT found !");
            if(await _repo.ExistsByName(itemUpdateDTO.Name))
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{itemUpdateDTO.Name}' is already registered !");


            Console.WriteLine($"--> UPDATING item '{item.Id}': '{item.Name}'......");


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


            Console.WriteLine($"--> DELETING item '{id}'......");


            var result = await _repo.DeleteItem(item);

            if (result.State != EntityState.Deleted || _repo.SaveChanges() < 1)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item with id '{id}' was NOT removed from DB !");

            item = (Item)result.Entity;

            return _resultFact.Result(_mapper.Map<ItemReadDTO>(item), true);
        }

    }
}
