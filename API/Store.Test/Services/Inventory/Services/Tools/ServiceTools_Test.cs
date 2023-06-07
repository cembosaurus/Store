using Inventory.Models;
using Business.Libraries.ServiceResult;
using Inventory.Services.Tools;
using NUnit.Framework;

namespace Store.Test.Services.Inventory.Services.Tools
{
    [TestFixture]
    public class ServiceTools_Test
    {
        private ServiceTools _serviceTools;
        private CatalogueItem _catalogueItem;

        private const int _itemId = 1;
        private const int _accessoryItemId = 2;
        private const int _similarProductItemId = 3;
        private const int _newAccessoryItemId = 4;
        private const int _newSimilarProductItemId = 5;
        private List<int> _accessories;
        private List<int> _similarProducts;
        private List<int> _newAccessories;
        private List<int> _newSimilarProducts;


        [SetUp]
        public void SetUp()
        {
            _serviceTools = new ServiceTools(new ServiceResultFactory());

            _catalogueItem = new CatalogueItem {
                ItemId = _itemId,
                Description = "catalogue item test description",
                Instock = 10,
                ItemPrice = new ItemPrice {
                    ItemId = _itemId,
                    SalePrice = 12.3,
                    RRP = 45.6,
                    DiscountPercent = 7
                },
                Item = new Item {
                    Id = _itemId,
                    Name = "item test name",
                    Description = "item test description",
                    PhotoURL = "item photo test url",
                    Archived = false
                },
                Accessories = new List<AccessoryItem> {
                    new AccessoryItem
                    {
                        ItemId = _itemId,
                        AccessoryItemId = _accessoryItemId
                    }
                },
                SimilarProducts = new List<SimilarProductItem> {
                    new SimilarProductItem
                    {
                        ItemId = _itemId,
                        SimilarProductItemId = _similarProductItemId
                    }
                }
            };

            _accessories = new List<int> { _accessoryItemId };
            _similarProducts = new List<int> { _similarProductItemId }; 
            _newAccessories = new List<int> { _newAccessoryItemId };
            _newSimilarProducts = new List<int> { _newSimilarProductItemId };

        }




        // --> AddExtrasToCatalogItem()

        [Test]
        public void AddExtrasToCatalogItem_WhenCalled_ReturnsCatalogueItemWithAddedNewExtras()
        {
            var result = _serviceTools.AddExtrasToCatalogItem(_catalogueItem, _newAccessories, _newSimilarProducts);


            Assert.That(result.Data.Accessories, Is.Not.Null);
            Assert.That(result.Data.SimilarProducts, Is.Not.Null);
            Assert.That(result.Data.Accessories.Count, Is.EqualTo(2));
            Assert.That(result.Data.SimilarProducts.Count, Is.EqualTo(2));
            Assert.That(result.Data.Accessories.Any(a => a.AccessoryItemId == _newAccessoryItemId));
            Assert.That(result.Data.SimilarProducts.Any(si => si.SimilarProductItemId == _newSimilarProductItemId));
            Assert.True(result.Status);
        }



        [Test]
        public void AddExtrasToCatalogItem_DuplicitExtrasEntered_ReturnsCatalogueItemWithAddedNewExtrasAndMessageWarning()
        {
            _newAccessories.Add(_accessoryItemId);
            _newSimilarProducts.Add(_similarProductItemId);

            var result = _serviceTools.AddExtrasToCatalogItem(_catalogueItem, _newAccessories, _newSimilarProducts);


            Assert.That(result.Data.Accessories, Is.Not.Null);
            Assert.That(result.Data.SimilarProducts, Is.Not.Null);
            Assert.That(result.Data.Accessories.Count, Is.EqualTo(2));
            Assert.That(result.Data.SimilarProducts.Count, Is.EqualTo(2));
            Assert.That(result.Data.Accessories.Any(a => a.AccessoryItemId == _newAccessoryItemId));
            Assert.That(result.Data.SimilarProducts.Any(si => si.SimilarProductItemId == _newSimilarProductItemId));
            Assert.That(result.Message, Does.StartWith($"Extras '{_accessoryItemId},{_similarProductItemId}' are already in catalogue item '{_catalogueItem.ItemId}'"));
            Assert.True(result.Status);
        }



        [Test]
        public void AddExtrasToCatalogItem_CatalogueItemNotProvided_ReturnsFailMessage()
        {
            var result = _serviceTools.AddExtrasToCatalogItem(null, _newAccessories, _newSimilarProducts);


            Assert.That(result.Message, Is.EqualTo("Catalogue item was not provided !"));
            Assert.False(result.Status);
        }


        [Test]
        public void AddExtrasToCatalogItem_ExtrasNotProvided_ReturnsFailMessage()
        {
            var result = _serviceTools.AddExtrasToCatalogItem(_catalogueItem, null, null);


            Assert.That(result.Message, Is.EqualTo("Extras were not provided !"));
            Assert.False(result.Status);
        }




        // --> RemoveExtrasFromCatalogItem()

        [Test]
        public void RemoveExtrasFromCatalogItem_WhenCalled_ReturnsCatalogueItemWithRemovedExtras()
        {
            var result = _serviceTools.RemoveExtrasFromCatalogItem(_catalogueItem, _accessories, _similarProducts);


            Assert.That(result.Data.Accessories, Is.Not.Null);
            Assert.That(result.Data.SimilarProducts, Is.Not.Null);
            Assert.That(result.Data.Accessories, Is.Empty);
            Assert.That(result.Data.SimilarProducts, Is.Empty);
            Assert.True(result.Status);
        }



        [Test]
        public void RemoveExtrasFromCatalogItem_SomeExtrasNotPresentInCatalogueItem_ReturnsCatalogueItemWithRemovedExtrasAndMessageWarning()
        {
            // accessories removed, similar items not removed:
            var result = _serviceTools.RemoveExtrasFromCatalogItem(_catalogueItem, _accessories, _newSimilarProducts);


            Assert.That(result.Data.Accessories, Is.Not.Null);
            Assert.That(result.Data.SimilarProducts, Is.Not.Null);
            Assert.That(result.Data.Accessories, Is.Empty);
            Assert.That(result.Data.SimilarProducts.Count, Is.EqualTo(1));
            Assert.That(result.Message, Does.StartWith($"Can't remove non-existing extras '{_newSimilarProductItemId}' from catalogue item '{_catalogueItem.ItemId}'"));
            Assert.True(result.Status);
        }


        [Test]
        public void RemoveExtrasFromCatalogItem_CatalogueItemNotProvided_ReturnsFailMessage()
        {
            var result = _serviceTools.RemoveExtrasFromCatalogItem(null, _newAccessories, _newSimilarProducts);


            Assert.That(result.Message, Is.EqualTo("Catalogue item was not provided !"));
            Assert.False(result.Status);
        }


        [Test]
        public void RemoveExtrasFromCatalogItem_ExtrasNotProvided_ReturnsFailMessage()
        {
            var result = _serviceTools.RemoveExtrasFromCatalogItem(_catalogueItem, null, null);


            Assert.That(result.Message, Is.EqualTo("Extras were not provided !"));
            Assert.False(result.Status);
        }

    }
}
