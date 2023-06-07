using AutoMapper;
using Business.AMQP.Custom;
using Business.Inventory.DTOs.Item;
using Business.Libraries.ServiceResult.Interfaces;
using Inventory.Consumer.Data.Repositories.Interfaces;
using Inventory.Consumer.Models;
using Inventory.Consumer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Inventory.Consumer.Services
{
    public class ItemService : IItemService
    {

        private readonly IItemRepository _itemRepo;
        private readonly IServiceResultFactory _resultFact;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepo, IServiceResultFactory resultFact, IMapper mapper)
        {
            _itemRepo = itemRepo;
            _resultFact = resultFact;
            _mapper = mapper;
        }




        public async Task<IServiceResult<object>> Request(string reqMethod, string reqParam)
        {        

            var reqMethodId = System.Text.Json.JsonSerializer.Deserialize<int>(reqMethod);


            switch (reqMethodId)
            {
                case (int)RequestMethods.Item.GetAll:
                case (int)RequestMethods.Item.Get:
                    {
                        var param = JsonConvert.DeserializeAnonymousType(reqParam, new { ItemIds = new List<int>().AsEnumerable() });

                        return await GetItems(param.ItemIds);
                    }
                case (int)RequestMethods.Item.GetById:
                    {
                        var param = JsonConvert.DeserializeAnonymousType(reqParam, new { Id = 0 });

                        return await GetItemById(param.Id);
                    }
                case (int)RequestMethods.Item.Add:
                    {
                        var param = JsonConvert.DeserializeAnonymousType(reqParam, new { Item = new ItemCreateDTO() });

                        return await AddItem(param.Item);
                    }
                case (int)RequestMethods.Item.Update:
                    {
                        var param = JsonConvert.DeserializeAnonymousType(reqParam, new { Id = 0, Item = new ItemUpdateDTO() });

                        return await UpdateItem(param.Id, param.Item);
                    }
                case (int)RequestMethods.Item.Remove:
                    {
                        var param = JsonConvert.DeserializeAnonymousType(reqParam, new { Id = 0 });

                        return await DeleteItem(param.Id);
                    }
                default: 
                    {
                        return _resultFact.Result<object>(null, false, $"Request FAILED. Wrong request method ID: {reqMethod} !");
                    }
            }

        }



        // arg validation handled in DTO class (fluent validation)


        private async Task<IServiceResult<object>> GetItems(IEnumerable<int> itemIds = null)
        {
            Console.WriteLine($"--> GETTING items ......");

            var message = "";


            var items = await _itemRepo.GetItems(itemIds);

            if (!items.Any())
                message = "NO items found !";

            return _resultFact.Result<object>(_mapper.Map<IEnumerable<ItemReadDTO>>(items), true, message);
        }



        private async Task<IServiceResult<object>> GetItemById(int id)
        {
            Console.WriteLine($"--> GETTING item '{id}' ......");

            var message = "";


            var item = await _itemRepo.GetItemById(id);

            if (item == null)
                message = $"Item '{id}' was NOT found !";

            return _resultFact.Result<object>(_mapper.Map<ItemReadDTO>(item), true, message);
        }



        private async Task<IServiceResult<object>> AddItem(ItemCreateDTO itemCreateDTO)
        {
            if (await _itemRepo.ExistsByName(itemCreateDTO.Name))
                return _resultFact.Result<object>(null, false, $"Item '{itemCreateDTO.Name}' already EXISTS !");


            Console.WriteLine($"--> ADDING item '{itemCreateDTO.Name}'......");


            var item = _mapper.Map<Item>(itemCreateDTO);

            var resultState = await _itemRepo.AddItem(item);


            if (resultState != EntityState.Added || _itemRepo.SaveChanges() < 1)
                return _resultFact.Result<object>(null, false, $"Item '{itemCreateDTO.Name}' was NOT created");

            return _resultFact.Result<object>(_mapper.Map<ItemReadDTO>(item), true);
        }



        private async Task<IServiceResult<object>> UpdateItem(int id, ItemUpdateDTO itemUpdateDTO)
        {
            var item = await _itemRepo.GetItemById(id);

            if (item == null)
                return _resultFact.Result<object>(null, false, $"Item '{id}' NOT found !");
            if (await _itemRepo.ExistsByName(itemUpdateDTO.Name))
                return _resultFact.Result<object>(null, false, $"Item with name: '{itemUpdateDTO.Name}' already exists !");


            Console.WriteLine($"--> UPDATING item '{item.Id}': '{item.Name}'......");


            _mapper.Map(itemUpdateDTO, item);

            if (_itemRepo.SaveChanges() < 1)
                return _resultFact.Result<object>(null, false, $"Item '{id}': changes were NOT saved into DB !");

            return _resultFact.Result<object>(_mapper.Map<ItemReadDTO>(item), true);
        }



        private async Task<IServiceResult<object>> DeleteItem(int id)
        {
            var item = await _itemRepo.GetItemById(id);

            if (item == null)
                return _resultFact.Result<object>(null, false, $"Item '{id}' NOT found !");
            if (item.Archived)
                return _resultFact.Result<object>(null, false, $"Item '{id}' can NOT be deleted because it is ARCHIVED !");


            Console.WriteLine($"--> DELETING item '{id}'......");


            var resultState = await _itemRepo.DeleteItem(item);

            if (resultState != EntityState.Deleted || _itemRepo.SaveChanges() < 1)
                return _resultFact.Result<object>(null, false, $"Item with id '{id}' was NOT removed from DB !");

            return _resultFact.Result<object>(_mapper.Map<ItemReadDTO>(item), true);
        }
    }
}
