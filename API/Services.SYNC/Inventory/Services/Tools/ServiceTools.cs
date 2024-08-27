using Inventory.Models;
using Business.Libraries.ServiceResult.Interfaces;
using Inventory.Services.Tools.Interfaces;

namespace Inventory.Services.Tools
{
    public class ServiceTools: IServiceTools
    {

        private readonly IServiceResultFactory _resultFact;


        public ServiceTools(IServiceResultFactory resultFact)
        {
            _resultFact = resultFact;
        }


        public IServiceResult<CatalogueItem> AddExtrasToCatalogItem(CatalogueItem catalogueItem, IEnumerable<int> Accessories, IEnumerable<int>? SimilarProducts)
        {
            var intersected = new List<int>();

            if (catalogueItem == null)
                return _resultFact.Result<CatalogueItem>(null, false, "Catalogue item was not provided !");

            if (Accessories == null && SimilarProducts == null)
                return _resultFact.Result<CatalogueItem>(null, false, "Extras were not provided !");

            if (Accessories != null && Accessories.Any())
            {
                if (catalogueItem.Accessories == null)
                    catalogueItem.Accessories = new List<AccessoryItem>();

                intersected.AddRange(Accessories.Intersect(catalogueItem.Accessories.Select(s => s.AccessoryItemId)));                
                
                Accessories = Accessories.Except(catalogueItem.Accessories.Select(s => s.AccessoryItemId)).ToList();


                foreach (var aId in Accessories)
                {
                    catalogueItem.Accessories.Add(new AccessoryItem
                    {
                        ItemId = catalogueItem.ItemId,
                        AccessoryItemId = aId
                    });

                    catalogueItem.Accessories = catalogueItem.Accessories.Distinct().ToList();
                }
            }

            if (SimilarProducts != null && SimilarProducts.Any())
            {
                if (catalogueItem.SimilarProducts == null)
                    catalogueItem.SimilarProducts = new List<SimilarProductItem>();

                intersected.AddRange(SimilarProducts.Intersect(catalogueItem.SimilarProducts.Select(s => s.SimilarProductItemId)));

                SimilarProducts = SimilarProducts.Except(catalogueItem.SimilarProducts.Select(s => s.SimilarProductItemId)).ToList();


                foreach (var spId in SimilarProducts)
                {
                    catalogueItem.SimilarProducts.Add(new SimilarProductItem
                    {
                        ItemId = catalogueItem.ItemId,
                        SimilarProductItemId = spId
                    });

                    catalogueItem.SimilarProducts = catalogueItem.SimilarProducts.Distinct().ToList();
                }
            }

            return _resultFact.Result(catalogueItem, true, intersected.Any() ? $"Extras '{string.Join(",", intersected)}' are already in catalogue item '{catalogueItem.ItemId}': '{catalogueItem.Item.Name}'. They were not added into DB !" : "");
        }



        public IServiceResult<CatalogueItem> RemoveExtrasFromCatalogItem(CatalogueItem catalogueItem, IEnumerable<int> Accessories, IEnumerable<int>? SimilarProducts)
        {
            var missed = new List<int>();

            if (catalogueItem == null)
                return _resultFact.Result<CatalogueItem>(null, false, "Catalogue item was not provided !");

            if (Accessories == null && SimilarProducts == null)
                return _resultFact.Result<CatalogueItem>(null, false, "Extras were not provided !");


            if ((catalogueItem.Accessories != null && catalogueItem.Accessories.Any()) && (Accessories != null && Accessories.Any()))
            {
                foreach (var aId in Accessories)
                {
                    var accessoryToRemove = catalogueItem.Accessories.FirstOrDefault(r => r.AccessoryItemId == aId);

                    if (accessoryToRemove != null)
                    {
                        catalogueItem.Accessories.Remove(accessoryToRemove);
                    }
                    else { missed.Add(aId); }
                }
            }

            if ((catalogueItem.SimilarProducts != null && catalogueItem.SimilarProducts.Any()) && (SimilarProducts != null && SimilarProducts.Any()))
            {
                foreach (var spId in SimilarProducts)
                {
                    var accessoryToRemove = catalogueItem.SimilarProducts.FirstOrDefault(r => r.SimilarProductItemId == spId);

                    if (accessoryToRemove != null)
                    {
                        catalogueItem.SimilarProducts.Remove(accessoryToRemove);
                    }
                    else { missed.Add(spId); }
                }
            }

            return _resultFact.Result(catalogueItem, true, missed.Any() ? $"Can't remove non-existing extras '{string.Join(",", missed)}' from catalogue item '{catalogueItem.ItemId}': '{catalogueItem.Item.Name}'. They were ignored !" : "");
        }

    }
}
