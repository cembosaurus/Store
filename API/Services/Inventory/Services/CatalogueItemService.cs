using AutoMapper;
using Business.Inventory.DTOs.CatalogueItem;
using Inventory.Models;
using Business.Libraries.ServiceResult.Interfaces;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Inventory.Data.Repositories.Interfaces;



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
            var catalogueItems = await _catalogueItemRepo.GetCatalogueItems(itemIds);

            return _resultFact.Result(_mapper.Map<IEnumerable<CatalogueItemReadDTO>>(catalogueItems), true, !catalogueItems.Any() ? "NO catalogue items found !" : "");
        }


        public async Task<IServiceResult<CatalogueItemReadDTO>> GetCatalogueItemById(int id)
        {
            var message = "";
            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemById(id);

            if (catalogueItem == null)
            {
                message = $"Catalogue Item with id '{id}' NOT found";

                if (await _itemRepo.ExistsById(id))
                    message += Environment.NewLine + $", but Item with id '{id}' exists and it's not registered in catalogue !";
            }

            return _resultFact.Result(_mapper.Map<CatalogueItemReadDTO>(catalogueItem), true, message);
        }



        public async Task<IServiceResult<bool>> ExistsCatalogueItemById(int id)
        {
            var catalogueItemResult = await _catalogueItemRepo.ExistsById(id);

            return _resultFact.Result(catalogueItemResult, true, catalogueItemResult ? "" : $"Catalogue item '{id}' does NOT exist !");
        }



        public async Task<IServiceResult<CatalogueItemReadDTOWithExtras>> GetCatalogueItemWithExtrasById(int id)
        {
            var message = "";
            var catalogueItemWithExtras = await _catalogueItemRepo.GetCatalogueItemWithExtrasById(id);

            if(catalogueItemWithExtras == null)
                return _resultFact.Result<CatalogueItemReadDTOWithExtras>(null, true, $"Catalogue item with item id '{id}' NOT found !");

            var catalogueItemWithExtrasDTO = _mapper.Map<CatalogueItemReadDTOWithExtras>(catalogueItemWithExtras);


            var accessories = await _catalogueItemRepo.GetAccessoriesForCatalogueItem(catalogueItemWithExtras);

            if (accessories == null || !accessories.Any())
            {
                message += Environment.NewLine + $"Accessoriesw for item '{catalogueItemWithExtras.ItemId}' not found.";
            }
            else
            {
                catalogueItemWithExtrasDTO.AccessoryItems = _mapper.Map<IEnumerable<CatalogueItemReadDTO>>(accessories);
            }


            var similarProducts = await _catalogueItemRepo.GetSimilarProductsForCatalogueItem(catalogueItemWithExtras);

            if (similarProducts == null || !similarProducts.Any())
            {
                message += Environment.NewLine + $"Similar products for item '{catalogueItemWithExtras.ItemId}' not found.";
            }
            else 
            {
                catalogueItemWithExtrasDTO.SimilarProductItems = _mapper.Map<IEnumerable<CatalogueItemReadDTO>>(similarProducts);
            }


            return _resultFact.Result(catalogueItemWithExtrasDTO, true, message);
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> CreateCatalogueItem(int itemId, CatalogueItemCreateDTO catalogueItemCreateDTO)
        {
            if (await _catalogueItemRepo.ExistsById(itemId))
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Catalogue item with id '{itemId}' already EXISTS !");

            var item = await _itemRepo.GetItemById(itemId);

            if (item == null)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Item with id '{itemId}' NOT found !");

            catalogueItemCreateDTO.Instock = (catalogueItemCreateDTO.Instock == null || catalogueItemCreateDTO.Instock < 0) ? 0 : catalogueItemCreateDTO.Instock;

            var catalogueItem = _mapper.Map<CatalogueItem>(item);

            _mapper.Map(catalogueItemCreateDTO, catalogueItem);

            var resultState = await _catalogueItemRepo.CreateCatalogueItem(catalogueItem);

            if(resultState != EntityState.Added || _catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Catalogue item with item id '{itemId}' was NOT created !");

            return _resultFact.Result(_mapper.Map<CatalogueItemReadDTO>(catalogueItem), true);
        }



        public async Task<IServiceResult<ExtrasReadDTO>> AddExtrasToCatalogueItem(int id, ExtrasAddDTO extrasAddDTO)
        {
            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemWithExtrasById(id);

            if(catalogueItem == null)
                return _resultFact.Result<ExtrasReadDTO>(null, false, $"Catalogue item with id '{id}' NOT found !");

            catalogueItem.Accessories = catalogueItem.Accessories ?? new List<AccessoryItem>();
            catalogueItem.SimilarProducts = catalogueItem.SimilarProducts ?? new List<SimilarProductItem>();

            var availableAccessories = new List<AccessoryItem>();

            if (extrasAddDTO.AccessoryIds != null && extrasAddDTO.AccessoryIds.Any())
            {
                extrasAddDTO.AccessoryIds = extrasAddDTO.AccessoryIds
                    .Distinct()
                    .Except(catalogueItem.Accessories.Select(s => s.AccessoryItemId))
                    .AsQueryable();

                var accessoriesToAdd = extrasAddDTO.AccessoryIds.Select(s => new AccessoryItem { AccessoryItemId = s, ItemId = id });

                foreach (var a in accessoriesToAdd)
                    if(await _catalogueItemRepo.ExistsById(a.AccessoryItemId))
                        availableAccessories.Add(a);

                await _catalogueItemRepo.AddAccessoriesToCatalogueItem(id, availableAccessories);
            }


            var availableSimilarProducts = new List<SimilarProductItem>();

            if (extrasAddDTO.SimilarProductIds != null && extrasAddDTO.SimilarProductIds.Any())
            {
                extrasAddDTO.SimilarProductIds = extrasAddDTO.SimilarProductIds
                    .Distinct()
                    .Except(catalogueItem.SimilarProducts.Select(s => s.SimilarProductItemId))
                    .AsQueryable();

                var similarProductsToAdd = extrasAddDTO.SimilarProductIds.Select(s => new SimilarProductItem { SimilarProductItemId = s, ItemId = id });

                foreach(var s in similarProductsToAdd)
                    if(await _catalogueItemRepo.ExistsById(s.SimilarProductItemId))
                        availableSimilarProducts.Add(s);

                await _catalogueItemRepo.AddSimilarProductsToCatalogueItem(id, availableSimilarProducts);
            }


            if (_catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result<ExtrasReadDTO>(null, false, $"Extras for catalogue item with item id '{catalogueItem.ItemId}' were NOT added !");

            var result = new ExtrasReadDTO 
            { 
                Accessories = availableAccessories.Select(aa => aa.AccessoryItemId), 
                SimilarProducts = availableSimilarProducts.Select(asp => asp.SimilarProductItemId) 
            };

            return _resultFact.Result(result, true);
        }



        public async Task<IServiceResult<CatalogueItemReadDTO>> UpdateCatalogueItem(int itemId, CatalogueItemUpdateDTO catalogueItemUpdateDTO)
        {
            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemById(itemId);

            if (catalogueItem == null)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Catalogue item with id '{itemId}' NOT found !");

            catalogueItemUpdateDTO.Instock = (catalogueItemUpdateDTO.Instock == null || catalogueItemUpdateDTO.Instock < 0) ? 0 : catalogueItemUpdateDTO.Instock;

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

            var resultState = await _catalogueItemRepo.RemoveCatalogueItem(catalogueItemToRemove);

            if (resultState != EntityState.Deleted || _catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result<CatalogueItemReadDTO>(null, false, $"Catalogue item with item id '{catalogueItemToRemove.ItemId}' was NOT removed !");

            return _resultFact.Result(_mapper.Map<CatalogueItemReadDTO>(catalogueItemToRemove), true);
        }



        public async Task<IServiceResult<ExtrasReadDTO>> RemoveExtrasFromCatalogueItem(int id, ExtrasRemoveDTO extrasRemoveDTO)
        {
            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemWithExtrasById(id);

            if (catalogueItem == null)
                return _resultFact.Result<ExtrasReadDTO>(null, false, $"Catalogue item with id '{id}' NOT found !");

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

            return _resultFact.Result(new ExtrasReadDTO { Accessories = extrasRemoveDTO.AccessoryIds, SimilarProducts = extrasRemoveDTO.SimilarProductIds }, true);
        }



        public async Task<IServiceResult<int>> GetInstockCount(int id)
        {
            if (!await _catalogueItemRepo.ExistsById(id))
            {
                var message = $"Catalogue Item with id '{id}' NOT found";

                if (await _itemRepo.ExistsById(id))
                    message += $", but Item with id '{id}' exists and it's not registered in catalogue !";

                return _resultFact.Result(0, false, message);
            }

            var instock = await _catalogueItemRepo.GetInstockCount(id);

            return _resultFact.Result(instock, true);
        }



        public async Task<IServiceResult<int>> RemoveFromStockAmount(int id, int amount)
        {
            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemById(id);

            if (catalogueItem == null)
            {
                var message = $"Catalogue Item with id '{id}' NOT found";

                if (await _itemRepo.ExistsById(id))
                    return _resultFact.Result(0, false, message + ", but Item with this id exists !");

                return _resultFact.Result(0, false, message + " !");
            }

            var instock = await _catalogueItemRepo.GetInstockCount(id);

            catalogueItem.Instock = amount > instock ? 0 : catalogueItem.Instock - amount;

            if (_catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result(0, false, $"Catalogue item '{id}' instock amount was NOT changed !");

            return _resultFact.Result(catalogueItem.Instock, true, instock - amount < 0 ? $"Insufficient amount in stock ! Only {instock} catalogue items were removed from stock !" : "");
        }



        public async Task<IServiceResult<int>> AddAmountToStock(int itemId, int amount)
        {
            if (amount < 0)
                return _resultFact.Result(0, false, "Only positive number can be added to stock amount !");

            var catalogueItem = await _catalogueItemRepo.GetCatalogueItemById(itemId);

            if (catalogueItem == null)
            {
                var message = $"Catalogue Item with id '{itemId}' NOT found";

                if (await _itemRepo.ExistsById(itemId))
                    return _resultFact.Result(0, false, message + ", but Item with this id exists !");

                return _resultFact.Result(0, false, message + " !");
            }

            catalogueItem.Instock += amount;

            if (_catalogueItemRepo.SaveChanges() < 1)
                return _resultFact.Result(0, false, $"Catalogue item '{itemId}' instock amount was NOT changed !");

            return _resultFact.Result(catalogueItem.Instock, true);
        }
    }
}
