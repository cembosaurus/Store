using Business.Inventory.DTOs.ItemPrice;
using Inventory.Models;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;
using Moq;
using NUnit.Framework;
using Ordering.HttpServices.Interfaces;
using Ordering.OrderingBusinessLogic;
using Services.Ordering.Models;

namespace Store.Test.Services.Ordering.OrderingBusinessLogic
{
    [TestFixture]
    public class OrderingTools_Test
    {
        private OrderingTools _orderingHelper;

        private Mock<IHttpInventoryService> _httpInventoryService = new Mock<IHttpInventoryService>();
        private Mock<IHttpSchedulerService> _httpSchedulerService = new Mock<IHttpSchedulerService>();
        private IServiceResultFactory _resultFact = new ServiceResultFactory();

        private Cart _cart;
        private CartItem _cartItem1, _cartItem2, _cartItem3;
        private Guid _cartId = new Guid();
        private double _cartTotal;
        private int _item1_Id = 1, _item2_Id = 2, _item3_Id = 3;
        private int _item1_CountInStock = 100;
        private ItemPrice _itemPrice1, _itemPrice2, _itemPrice3;
        private ItemPriceReadDTO _itemPrice1ReadDTO, _itemPrice2ReadDTO, _itemPrice3ReadDTO;
        private IEnumerable<CartItem> _cartItems_List;


        [SetUp]
        public void Setup()
        {
            _cartItem1 = new CartItem { CartId = _cartId, ItemId = _item1_Id, Amount = 10};
            _cartItem2 = new CartItem { CartId = _cartId, ItemId = _item2_Id, Amount = 20};
            _cartItem3 = new CartItem { CartId = _cartId, ItemId = _item3_Id, Amount = 30};
            _cartItems_List = new List<CartItem> { _cartItem1, _cartItem2, _cartItem3 };

            _cart = new Cart 
            {
                CartId = _cartId,
                CartItems = _cartItems_List.ToList()
            };

            _itemPrice1 = new ItemPrice { ItemId = _item1_Id, SalePrice = 100, RRP = 101, DiscountPercent = 10 };
            _itemPrice1ReadDTO = new ItemPriceReadDTO 
            { 
                ItemId = _itemPrice1 .ItemId, 
                SalePrice = _itemPrice1.SalePrice, 
                RRP = _itemPrice1.RRP, 
                DiscountPercent = _itemPrice1.DiscountPercent 
            };
            _itemPrice2 = new ItemPrice { ItemId = _item2_Id, SalePrice = 200, RRP = 202, DiscountPercent = 20 };
            _itemPrice2ReadDTO = new ItemPriceReadDTO
            {
                ItemId = _itemPrice2.ItemId,
                SalePrice = _itemPrice2.SalePrice,
                RRP = _itemPrice2.RRP,
                DiscountPercent = _itemPrice2.DiscountPercent
            };
            _itemPrice3 = new ItemPrice { ItemId = _item3_Id, SalePrice = 300, RRP = 303, DiscountPercent = 30 };
            _itemPrice3ReadDTO = new ItemPriceReadDTO
            {
                ItemId = _itemPrice3.ItemId,
                SalePrice = _itemPrice3.SalePrice,
                RRP = _itemPrice3.RRP,
                DiscountPercent = _itemPrice3.DiscountPercent
            };

            _cartTotal = 
                  _cart.CartItems.ElementAt(0).Amount * _itemPrice1.SalePrice 
                + _cart.CartItems.ElementAt(1).Amount * _itemPrice2.SalePrice 
                + _cart.CartItems.ElementAt(2).Amount * _itemPrice3.SalePrice;

            _orderingHelper = new OrderingTools(_resultFact, _httpInventoryService.Object, _httpSchedulerService.Object);
        }





        // UpdateCartTotal()
        
        [Test]
        public void UpdateCartTotal_WhenCalled_UpdatesCartTotal()
        {
            _httpInventoryService.Setup(i => i.GetItemPriceById(_cartItem1.ItemId)).Returns(Task.FromResult(_resultFact.Result(_itemPrice1ReadDTO, true)));
            _httpInventoryService.Setup(i => i.GetItemPriceById(_cartItem2.ItemId)).Returns(Task.FromResult(_resultFact.Result(_itemPrice2ReadDTO, true)));
            _httpInventoryService.Setup(i => i.GetItemPriceById(_cartItem3.ItemId)).Returns(Task.FromResult(_resultFact.Result(_itemPrice3ReadDTO, true)));


            var result = _orderingHelper.UpdateCartTotal(_cart).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.EqualTo(_cartTotal));
        }


        [Test]
        public void UpdateCartTotal_CartNotUpdated_ReturnsMessage()
        {
            var message = "Test Message";

            _httpInventoryService.Setup(i => i.GetItemPriceById(_cartItem1.ItemId)).Returns(Task.FromResult(_resultFact.Result(_itemPrice1ReadDTO, true)));
            _httpInventoryService.Setup(i => i.GetItemPriceById(_cartItem2.ItemId)).Returns(Task.FromResult(_resultFact.Result(_itemPrice2ReadDTO, false, message)));
            _httpInventoryService.Setup(i => i.GetItemPriceById(_cartItem3.ItemId)).Returns(Task.FromResult(_resultFact.Result(_itemPrice3ReadDTO, true)));


            var result = _orderingHelper.UpdateCartTotal(_cart).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Is.EqualTo(_cartTotal - (_itemPrice2ReadDTO.SalePrice * _cart.CartItems.ElementAt(1).Amount)));
            Assert.That(result.Message, Is.EqualTo($"Cart total was NOT updated by Item '{_cartItem2.ItemId}' ! Reason: '{message}'"));
        }


        [Test]
        public void UpdateCartTotal_CartNotProvided_ReturnsMessage()
        {
            var result = _orderingHelper.UpdateCartTotal(It.IsAny<Cart>()).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Cart was NOT provided !"));
        }


        [Test]
        public void UpdateCartTotal_CartItemsNotProvided_ReturnsMessage()
        {
            _cart.CartItems = new List<CartItem>();

            var result = _orderingHelper.UpdateCartTotal(_cart).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"NO items found in cart. Total is: '{_cart.Total}'"));
        }



        // AddItemsToCart()

        [Test]
        public void AddItemsToCart_WhenCalled_AddsItemsToCart()
        {
            var newCartItems = new List<CartItem> { new CartItem { ItemId = _item1_Id, Amount = 10 } };
            var item1_AmountInCartBeforeAdding = _cart.CartItems.ElementAt(0).Amount;

            _httpInventoryService.Setup(i => i.GetInstockCount(It.IsAny<int>())).Returns(Task.FromResult(_resultFact.Result(_item1_CountInStock, true)));


            var result = _orderingHelper.AddItemsToCart(_cart, newCartItems).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ElementAt(0).Amount, Is.EqualTo(newCartItems.ElementAt(0).Amount));
            Assert.That(_cart.CartItems.ElementAt(0).Amount, Is.EqualTo(item1_AmountInCartBeforeAdding + newCartItems.ElementAt(0).Amount));
        }


        [Test]
        public void AddItemsToCart_SomeItemsNotAddedToCart_ReturnsMessage()
        {
            var newCartItems = new List<CartItem> { new CartItem { ItemId = _item1_Id, Amount = 10 } };
            var item1_AmountInCartBeforeAdding = _cart.CartItems.ElementAt(0).Amount;

            _httpInventoryService.Setup(i => i.GetInstockCount(It.IsAny<int>())).Returns(Task.FromResult(_resultFact.Result(It.IsAny<int>(), false)));


            var result = _orderingHelper.AddItemsToCart(_cart, newCartItems).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Does.StartWith($"\r\nItem '{_cart.CartItems.ElementAt(0).ItemId}' was NOT added to cart ! Reason:"));
        }



        [Test]
        public void AddItemsToCart_NotEnoughAmountInStore_ReturnsMessage()
        {
            var newCartItems = new List<CartItem> { new CartItem { ItemId = _item1_Id, Amount = _item1_CountInStock + 1 } };
            var item1_AmountInCartBeforeAdding = _cart.CartItems.ElementAt(0).Amount;

            _httpInventoryService.Setup(i => i.GetInstockCount(It.IsAny<int>())).Returns(Task.FromResult(_resultFact.Result(_item1_CountInStock, true)));


            var result = _orderingHelper.AddItemsToCart(_cart, newCartItems).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Does.StartWith($"\r\nNot enough amount for item: '{_cart.CartItems.ElementAt(0).ItemId}'."));
        }



        // RemoveItemsFromCart()

        [Test]
        public void RemoveItemsFromCart_WhenCalled_RemovesItemsFGromCart()
        {
            var cartItemsToRemove = new List<CartItem> { new CartItem { ItemId = _item1_Id, Amount = 1 } };
            var item1_AmountInCartBeforeAdding = _cart.CartItems.ElementAt(0).Amount;


            var result = _orderingHelper.RemoveItemsFromCart(_cart, cartItemsToRemove).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.ElementAt(0).Amount, Is.EqualTo(cartItemsToRemove.ElementAt(0).Amount));
            Assert.That(_cart.CartItems.ElementAt(0).Amount, Is.EqualTo(item1_AmountInCartBeforeAdding - cartItemsToRemove.ElementAt(0).Amount));
        }


        [Test]
        public void RemoveItemsFromCart_ItemNotFoundInCart_ReturnsMessage()
        {
            var _nonExistingItem = new CartItem { ItemId = 4, Amount = 4 };

            var cartItemsToRemove = new List<CartItem> { _nonExistingItem };


            var result = _orderingHelper.RemoveItemsFromCart(_cart, cartItemsToRemove).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo($"\r\nItem '{_nonExistingItem.ItemId}' NOT found in cart '{_cart.UserId}'"));
        }


        [Test]
        public void RemoveItemsFromCart_TryingToRemoveMoreItemsThanArePresentOnCart_ReturnsMessage()
        {
            var tooHighAmountToRemove = _cartItem1.Amount + 1;

            var cartItemToRemove = _cart.CartItems.ElementAt(0).ItemId;

            var cartItemsToRemove = new List<CartItem> { new CartItem { ItemId = 1, Amount = tooHighAmountToRemove } };


            var result = _orderingHelper.RemoveItemsFromCart(_cart, cartItemsToRemove).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo($"\r\nAmount to remove '{tooHighAmountToRemove}' for item '{cartItemsToRemove.ElementAt(0).ItemId}' was higher than actual amount on cart and '0' amount has remained, and cart item '{cartItemToRemove}' was removed from cart!"));
        }



        // CartItemsLock()

        [Test]
        public void CartItemsLock_WhenCalled_LocksCartItems()
        {
            var itemsIds = new List<int> { 1,2,3};
            var cartItemsToLock = new CartItemsLockCreateDTO { ItemsIds = itemsIds};
            var cartItemsLocked = new CartItemsLockReadDTO { ItemsIds = itemsIds };

            _httpSchedulerService.Setup(ss => ss.CartItemsLock(It.IsAny<CartItemsLockCreateDTO>())).Returns(Task.FromResult(_resultFact.Result(cartItemsLocked, true)));


            var result = _orderingHelper.CartItemsLock(Guid.NewGuid(), itemsIds).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(itemsIds.Count()));
            Assert.That(result.Data.ElementAt(0), Is.EqualTo(itemsIds.ElementAt(0)));
        }


        [Test]
        public void CartItemsLock_ItemsNotLocked_ReturnsMessage()
        {
            var testMessage = "test message";
            var itemsIds = new List<int> { 1, 2, 3 };

            _httpSchedulerService.Setup(ss => ss.CartItemsLock(It.IsAny<CartItemsLockCreateDTO>()))
                .Returns(Task.FromResult(_resultFact.Result<CartItemsLockReadDTO>(null, false, testMessage)));


            var result = _orderingHelper.CartItemsLock(Guid.NewGuid(), itemsIds).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo(testMessage));
        }



        // CartItemsUnLock()

        [Test]
        public void CartItemsUnLock_WhenCalled_UnlocksCartItems()
        {
            var itemsIds = new List<int> { 1, 2, 3 };
            var cartItemsToUnLock = new CartItemsLockDeleteDTO { ItemsIds = itemsIds };
            var cartItemsUnLocked = new CartItemsLockReadDTO { ItemsIds = itemsIds };

            _httpSchedulerService.Setup(ss => ss.CartItemsUnLock(It.IsAny<CartItemsLockDeleteDTO>())).Returns(Task.FromResult(_resultFact.Result(cartItemsUnLocked, true)));


            var result = _orderingHelper.CartItemsUnLock(Guid.NewGuid(), itemsIds).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(itemsIds.Count()));
            Assert.That(result.Data.ElementAt(0), Is.EqualTo(itemsIds.ElementAt(0)));
        }


        [Test]
        public void CartItemsUnLock_ItemsNotUnLocked_ReturnsMessage()
        {
            var testMessage = "test message";
            var itemsIds = new List<int> { 1, 2, 3 };

            _httpSchedulerService.Setup(ss => ss.CartItemsUnLock(It.IsAny<CartItemsLockDeleteDTO>()))
                .Returns(Task.FromResult(_resultFact.Result<CartItemsLockReadDTO>(null, false, testMessage)));


            var result = _orderingHelper.CartItemsUnLock(Guid.NewGuid(), itemsIds).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo(testMessage));
        }



        // CreateOrderCode()

        [Test]
        public void CreateOrderCode_WhenCalled_CreatesOrderCode()
        {
            var cartId = 123;


            var result = _orderingHelper.CreateOrderCode(cartId);
            // OR/VXNlcjEyMy1EMjQwMzIwMjNUMTMxMDM3


            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Does.StartWith($"OR/"));
        }


        [Test]
        public void CreateOrderCode_CartIdOutOfRange_ReturnsMessage()
        {
            var result = _orderingHelper.CreateOrderCode(It.IsAny<int>());


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Cart Id must be positive integer larger than 0 !"));
        }



        // DecodeOrderCode()

        [Test]
        public void DecodeOrderCode_WhenCalled_DecodesOrderCode()
        {
            var orderCode = "OR/VXNlcjEyMy1EMjQwMzIwMjNUMTMxMDM3";


            var result = _orderingHelper.DecodeOrderCode(orderCode);
            // User123-D24032023T131037


            Assert.IsTrue(result.Status);
            Assert.That(result.Data, Does.StartWith("User"));
            Assert.That(result.Data, Does.Contain("-D"));
            Assert.That(result.Data, Does.Contain("T"));
        }

    }
}
