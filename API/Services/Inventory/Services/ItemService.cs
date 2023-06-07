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


            var resultState = await _repo.DeleteItem(item);

            if (resultState != EntityState.Deleted || _repo.SaveChanges() < 1)
                return _resultFact.Result<ItemReadDTO>(null, false, $"Item with id '{id}' was NOT removed from DB !");

            return _resultFact.Result(_mapper.Map<ItemReadDTO>(item), true);
        }

    }
}
