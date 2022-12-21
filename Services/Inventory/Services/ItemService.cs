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
            Console.WriteLine($"--> GETTING items ......");

            var message = "";


            var items = await _repo.GetItems(itemIds);

            if (!items.Any())
                message = "NO items found !";

            return _resultFact.Result(_mapper.Map<IEnumerable<ItemReadDTO>>(items), true, message);
        }



        public async Task<IServiceResult<ItemReadDTO>> GetItemById(int id)
        {
            Console.WriteLine($"--> GETTING item '{id}' ......");

            var message = "";


            var item = await _repo.GetItemById(id);

            if (item == null)
                message = $"Item '{id}' was NOT found !";

            return _resultFact.Result(_mapper.Map<ItemReadDTO>(item), true, message);
        }



        public async Task<IServiceResult<ItemReadDTO>> AddItem(ItemCreateDTO itemCreateDTO)
        {
            Console.WriteLine($"--> ADDING item '{itemCreateDTO.Name}'......");


            if (await _repo.ExistsByName(itemCreateDTO.Name))
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{itemCreateDTO.Name}' already EXISTS !");


            var result = await _repo.AddItem(_mapper.Map<Item>(itemCreateDTO));

            var item = (Item)result.Entity;


            if (result.State != EntityState.Added || _repo.SaveChanges() < 1)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item '{itemCreateDTO.Name}' was NOT created");

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
