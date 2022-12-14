using AutoMapper;
using Business.Inventory.DTOs.CatalogueItem;
using Business.Libraries.ServiceResult.Interfaces;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Inventory.Data.Repositories.Interfaces;
using Services.Inventory.Models;


namespace Inventory.Services
{
    public class CatalogueItemService: ICatalogueItemService
    {

        private readonly ICatalogueItemRepository _catalogueItemRepo;
        private readonly IItemRepository _itemRepo;
        private readonly IMapper _mapper;
        private readonly IServiceResultFactory _resultFact;

        public CatalogueItemService(ICatalogueItemRepository catalogueItemRepo, IItemRepository itemRepo,IMapper mapper, IServiceResultFactory resultFact)
        {
            _catalogueItemRepo = catalogueItemRepo;
            _itemRepo = itemRepo;
            _mapper = mapper;
            _resultFact = resultFact;
        }



        public async Task<IServiceResult<IEnumerable<CatalogueItemReadDTO>>> GetCatalogueItems(IEnumerable<int> itemIds = null)
        {
            Console.WriteLine($"--> GETTING catalogue items ......");


            var catalogueItems = await _catalogueItemRepo.GetCatalogueItems(itemIds);

            if (!catalogueItems.Any())
                return _resultFact.Result<IEnumerable<CatalogueItemReadDTO>>(null, false, "NO catalogue items found !");

            return _resultFact.Result(_mapper.Map<IEnumerable<CatalogueItemReadDTO>>(catalogueItems), true);
        }


        public async Task<IServiceResult<CatalogueItemReadDTO>> GetCatalogueItemById(int id)
        {
            Console.WriteLine($"--> GETTING catalogue item '{id}' ......");


            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemById(id);

            if (catalogueItem == null)
            {
                var message = $"Catalogue item with id '{id}' NOT found ";

                if (await _itemRepo.ExistsById(id))
                    return _resultFact.Result<CatalogueItemReadDTO>(null, false, message + "but Item with this id exists !");

                return _resultFact.Result<CatalogueItemReadDTO>(null, false, message + " !");
            }

            return _resultFact.Result(_mapper.Map<CatalogueItemReadDTO>(catalogueItem), true);
        }



        public async Task<IServiceResult<bool>> ExistsCatalogueItemById(int id)
        {
            Console.WriteLine($"--> EXISTS catalogue item '{id}' ......");


            var catalogueItem = await _catalogueItemRepo.ExistsById(id);

            if (!catalogueItem)
                return _resultFact.Result(false, false, $"Catalogue item '{id}' does NOT exist !");


            return _resultFact.Result(true, true);
        }



        public async Task<IServiceResult<CatalogueItemReadDTOWithExtras>> GetCatalogueItemWithExtrasById(int id)
        {
            var message = "";

            Console.WriteLine($"--> GETTING catalogue item '{id}' with extras ......");


            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemWithExtrasById(id);

            if(catalogueItem == null)
                return _resultFact.Result<CatalogueItemReadDTOWithExtras>(null, false, $"Catalogue item with item id '{id}' NOT found !");

            var catalogueItemDTO = _mapper.Map<CatalogueItemReadDTOWithExtras>(catalogueItem);


            var accessories = await _catalogueItemRepo.GetAccessoriesForCatalogueItem(catalogueItem);

            if (accessories == null || !accessories.Any())
            {
                message += Environment.NewLine + $"Accessoriesw for item '{catalogueItem.ItemId}' not found.";
            }
            else
            {
                catalogueItemDTO.AccessoryItems = _mapper.Map<IEnumerable<CatalogueItemReadDTO>>(accessories);
            }

            var similarProducts = await _catalogueItemRepo.GetSimilarProductsForCatalogueItem(catalogueItem);

            if (similarProducts == null || !similarProducts.Any())
            {
                message += Environment.NewLine + $"Similar prodicts for item '{catalogueItem.ItemId}' not found.";
            }
            else 
            {
                catalogueItemDTO.SimilarProductItems = _mapper.Map<IEnumerable<CatalogueItemReadDTO>>(similarProducts);
            }

            return _resultFact.Result(catalogueItemDTO, true, message);
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItemCreateDTO)
        {
            if (await _catalogueItemRepo.ExistsById(itemId))
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Catalogue item with id '{itemId}' already EXISTS !");

            var item = await _catalogueItemRepo.GetItemById(itemId);

            if (item == null)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Item with id '{itemId}' NOT found !");

            catalogueItemCreateDTO.Instock = (catalogueItemCreateDTO.Instock == null || catalogueItemCreateDTO.Instock < 0) ? 0 : catalogueItemCreateDTO.Instock;


            Console.WriteLine($"--> CREATING catalogue item '{itemId}'......");


            var catalogueItem = _mapper.Map<CatalogueItem>(item);

            _mapper.Map(catalogueItemCreateDTO, catalogueItem);

            var result = await _catalogueItemRepo.CreateCatalogueItem(catalogueItem);

            if(result.State != EntityState.Added || _catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Catalogue item with item id '{itemId}' was NOT created !");

            return _resultFact.Result(_mapper.Map<CatalogueItemReadDTO>(catalogueItem), true);
        }



        public async Task<IServiceResult<ExtrasReadDTO>> AddExtrasToCatalogueItem(int id, ExtrasAddDTO extrasAddDTO)
        {
            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemWithExtrasById(id);

            if(catalogueItem == null)
                return _resultFact.Result<ExtrasReadDTO>(null, false, $"Catalogue item with id '{id}' NOT found !");


            var message = "";

            Console.WriteLine($"--> ADDING extras to catalogue item '{catalogueItem.ItemId}': '{catalogueItem.Item.Name}'......");


            if (extrasAddDTO.AccessoryIds != null && extrasAddDTO.AccessoryIds.Any())
            {
                extrasAddDTO.AccessoryIds = extrasAddDTO.AccessoryIds
                    .Distinct()
                    .Where(id => _catalogueItemRepo.ExistsById(id).Result == true)
                    .Except(catalogueItem.Accessories
                    .Select(s => s.AccessoryItemId))
                    .ToList();

                var accessoriesToAdd = extrasAddDTO.AccessoryIds.Select(s => new AccessoryItem { AccessoryItemId = s, ItemId = id });

                await _catalogueItemRepo.AddAccessoriesToCatalogueItem(id, accessoriesToAdd);
            }

            if (extrasAddDTO.SimilarProductIds != null && extrasAddDTO.SimilarProductIds.Any())
            {
                extrasAddDTO.SimilarProductIds = extrasAddDTO.SimilarProductIds
                    .Distinct()
                    .Where(id => _catalogueItemRepo.ExistsById(id).Result == true)
                    .Except(catalogueItem.SimilarProducts
                    .Select(s => s.SimilarProductItemId))
                    .ToList();

                var similarProductsToAdd = extrasAddDTO.SimilarProductIds.Select(s => new SimilarProductItem { SimilarProductItemId = s, ItemId = id });

                await _catalogueItemRepo.AddSimilarProductsToCatalogueItem(id, similarProductsToAdd);
            }


            if (_catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result<ExtrasReadDTO>(null, false, $"Extras for catalogue item with item id '{catalogueItem.ItemId}' were NOT added !");

            return _resultFact.Result(_mapper.Map<ExtrasReadDTO>(new ExtrasReadDTO {Accessories = extrasAddDTO.AccessoryIds, SimilarProducts = extrasAddDTO.SimilarProductIds }), true);
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO catalogueItemUpdateDTO)
        {
            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemById(itemId);

            if (catalogueItem == null)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Catalogue item with id '{itemId}' NOT found !");

            catalogueItemUpdateDTO.Instock = (catalogueItemUpdateDTO.Instock == null || catalogueItemUpdateDTO.Instock < 0) ? 0 : catalogueItemUpdateDTO.Instock;


            Console.WriteLine($"--> UPDATING catalogue item '{catalogueItem.ItemId}': '{catalogueItem.Item.Name}'......");


            _mapper.Map(catalogueItemUpdateDTO, catalogueItem);

            if (_catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Catalogue item '{itemId}': changes were NOT saved into DB !");

            return _resultFact.Result(_mapper.Map<CatalogueItemReadDTO>(catalogueItem), true);
        }
   



        public async Task<IServiceResult<CatalogueItemReadDTO>> RemoveCatalogueItem(int id)
        {
            var catalogueItemToRemove = await _catalogueItemRepo.GetCatalogueItemById(id);

            if (catalogueItemToRemove == null)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Catalogue item with id '{id}' was NOT found !");


            Console.WriteLine($"--> REMOVING catalogue item '{catalogueItemToRemove.ItemId}': '{catalogueItemToRemove.Item.Name}'......");


            var result = await _catalogueItemRepo.RemoveCatalogueItem(catalogueItemToRemove);

            if (result.State != EntityState.Deleted || _catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Catalogue item with item id '{catalogueItemToRemove.ItemId}' was NOT removed !");

            return _resultFact.Result(_mapper.Map<CatalogueItemReadDTO>(catalogueItemToRemove), true);
        }



        public async Task<IServiceResult<ExtrasReadDTO>> RemoveExtrasFromCatalogueItem(int id, ExtrasRemoveDTO extrasRemoveDTO)
        {
            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemWithExtrasById(id);

            if (catalogueItem == null)
                return _resultFact.Result<ExtrasReadDTO>(null, false, $"Catalogue item with id '{id}' NOT found !");


            Console.WriteLine($"--> REMOVING extras from catalogue item '{id}' ......");


            if (extrasRemoveDTO.AccessoryIds != null && extrasRemoveDTO.AccessoryIds.Any())
            {
                var accessories = catalogueItem.Accessories.Where(a => extrasRemoveDTO.AccessoryIds.Any(aId => aId == a.AccessoryItemId)).ToList();

                await _catalogueItemRepo.RemoveAccessoriesFromCatalogueItem(id, accessories);
            }

            if (extrasRemoveDTO.SimilarProductIds != null && extrasRemoveDTO.SimilarProductIds.Any())
            {
                var similarProducts = catalogueItem.SimilarProducts.Where(sp => extrasRemoveDTO.SimilarProductIds.Any(spId => spId == sp.SimilarProductItemId)).ToList();

                await _catalogueItemRepo.RemoveSimilarProductsFromCatalogueItem(id, similarProducts);
            }

            if (_catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result<ExtrasReadDTO>(null, false, $"Extras for catalogue item with item id '{id}' were NOT removed !");

            return _resultFact.Result(_mapper.Map<ExtrasReadDTO>(new ExtrasReadDTO { Accessories = extrasRemoveDTO.AccessoryIds, SimilarProducts = extrasRemoveDTO.SimilarProductIds }), true);
        }



        public async Task<IServiceResult<int>> GetInstockCount(int id)
        {
            Console.WriteLine($"--> GETTING catalogue item '{id}' instock count ......");


            if (!await _catalogueItemRepo.ExistsById(id))
            {
                var message = $"Catalogue item with id '{id}' NOT found ";

                if (await _itemRepo.ExistsById(id))
                    return _resultFact.Result(0, false, message + "but Item with this id exists !");

                return _resultFact.Result(0, false, message + " !");
            }

            var instock = await _catalogueItemRepo.GetInstockCount(id);

            return _resultFact.Result(instock, true);
        }



        public async Task<IServiceResult<int>> RemoveFromStockAmount(int itemId, int amount)
        {
            Console.WriteLine($"--> REMOVING item '{itemId}' amount '{amount}' from stock ......");


            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemById(itemId);

            if (catalogueItem == null)
            {
                var message = $"Catalogue item with id '{itemId}' NOT found ";

                if (await _itemRepo.ExistsById(itemId))
                    return _resultFact.Result(0, false, message + "but Item with this id exists !");

                return _resultFact.Result(0, false, message + " !");
            }

            var instock = await _catalogueItemRepo.GetInstockCount(itemId);

            catalogueItem.Instock = amount > instock ? 0 : catalogueItem.Instock - amount;

            if (_catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result(0, false, $"Catalogue item '{itemId}' instock amount was NOT changed !");

            return _resultFact.Result(catalogueItem.Instock, true, instock - amount < 0 ? $"Insufficient amount in stock ! Only {instock} catalogue items were removed from stock !" : null);
        }



        public async Task<IServiceResult<int>> AddToStockAmount(int itemId, int amount)
        {
            if (amount < 0)
                return _resultFact.Result(0, false, "Only positive number can be added to stock amount !");


            Console.WriteLine($"--> ADDING item '{itemId}' amount '{amount}' to stock ......");


            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemById(itemId);

            if (catalogueItem == null)
            {
                var message = $"Catalogue item with id '{itemId}' NOT found ";

                if (await _itemRepo.ExistsById(itemId))
                    return _resultFact.Result(0, false, message + "but Item with this id exists !");

                return _resultFact.Result(0, false, message + " !");
            }

            catalogueItem.Instock += amount;

            if (_catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result(0, false, $"Catalogue item '{itemId}' instock amount was NOT changed !");

            return _resultFact.Result(catalogueItem.Instock, true);
        }
    }
}
