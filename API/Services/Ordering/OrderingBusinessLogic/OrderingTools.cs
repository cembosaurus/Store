using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;
using Ordering.HttpServices.Interfaces;
using Ordering.OrderingBusinessLogic.Interfaces;
using Services.Ordering.Models;
using System.Text;

namespace Ordering.OrderingBusinessLogic
{
    public class OrderingTools : ICartBusinessLogic, IOrderBusinessLogic
    {
        private readonly IHttpInventoryService _httpInventoryService;
        private readonly IHttpSchedulerService _httpSchedulerService;
        private readonly IServiceResultFactory _resultFact;

        public OrderingTools(IServiceResultFactory resultFact, IHttpInventoryService httpInventoryService, IHttpSchedulerService httpSchedulerService)
        {
            _httpInventoryService = httpInventoryService;
            _httpSchedulerService = httpSchedulerService;
            _resultFact = resultFact;
        }




        public async Task<IServiceResult<double>> UpdateCartTotal(Cart cart)
        {
            if (cart == null)
                return _resultFact.Result<double>(0, false, $"Cart was NOT provided !");
            if (!cart.CartItems.Any())
                return _resultFact.Result(cart.Total, false, $"NO items found in cart. Total is: '{cart.Total}'");


            var message = "";


            cart.Total = 0;

            foreach (var ci in cart.CartItems)
            {
                var priceResult = await _httpInventoryService.GetItemPriceById(ci.ItemId);

                if (priceResult == null || !priceResult.Status)
                {
                    message += $"Cart total was NOT updated by Item '{ci.ItemId}' ! Reason: '{priceResult?.Message}'";

                    continue;
                }

                var price = priceResult.Data.SalePrice;

                cart.Total += price * ci.Amount;
            }

            return _resultFact.Result(cart.Total, true, message);
        }



        public async Task<IServiceResult<IEnumerable<CartItem>>> AddItemsToCart(Cart cart, IEnumerable<CartItem> source)
        {
            if (cart == null)
                return _resultFact.Result<IEnumerable<CartItem>>(null, false, $"Cart for update was not provided !");
            if (source == null || !source.Any())
                return _resultFact.Result<IEnumerable<CartItem>>(null, false, $"Items to update cart were not provided !");


            var message = "";

            var result = new List<CartItem>();


            foreach (var itemToAdd in source)
            {
                var inStockResult = await _httpInventoryService.GetInstockCount(itemToAdd.ItemId);

                if (inStockResult == null || !inStockResult.Status)
                {
                    message += Environment.NewLine + $"Item '{itemToAdd.ItemId}' was NOT added to cart ! Reason: '{inStockResult?.Message}'";

                    continue;
                }

                if (inStockResult.Data < itemToAdd.Amount)
                {
                    itemToAdd.Amount = !inStockResult.Status ? 0 : inStockResult.Data;

                    message += Environment.NewLine + $"Not enough amount for item: '{itemToAdd.ItemId}'. Available amount '{(!inStockResult.Status ? inStockResult.Message : inStockResult.Data)}' was added to Cart.";
                }


                var newCartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ItemId = itemToAdd.ItemId,
                    Amount = itemToAdd.Amount,
                };


                var itemInCart = cart.CartItems.FirstOrDefault(ci => ci.ItemId == itemToAdd.ItemId);

                if (itemInCart == null)
                {
                    cart.CartItems.Add(newCartItem);
                }
                else
                {
                    itemInCart.Amount += itemToAdd.Amount;
                }

                result.Add(newCartItem);
            }

            return _resultFact.Result(result.AsEnumerable(), true, message);
        }



        public async Task<IServiceResult<IEnumerable<CartItem>>> RemoveItemsFromCart(Cart cart, IEnumerable<CartItem> source)
        {
            if (cart == null)
                return _resultFact.Result<IEnumerable<CartItem>>(null, false, $"Cart for update was not provided !");
            if (source == null || !source.Any())
                return _resultFact.Result<IEnumerable<CartItem>>(null, false, $"Items to delete from cart were not provided !");


            var message = "";

            var removedCartItem = new CartItem();
            var removedCartItems = new List<CartItem>();


            foreach (var si in source)
            {
                var itemInCart = cart.CartItems.FirstOrDefault(di => di.ItemId == si.ItemId);

                if (itemInCart == null)
                {
                    message += Environment.NewLine + $"Item '{si.ItemId}' NOT found in cart '{cart.UserId}'";

                    continue;
                }

                itemInCart.Amount -= si.Amount;

                if (itemInCart.Amount < 1)
                {
                    removedCartItem.Amount = itemInCart.Amount + si.Amount;
                    itemInCart.Amount = 0;
                    cart.CartItems.Remove(itemInCart);

                    message += Environment.NewLine + $"Amount to remove '{si.Amount}' for item '{si.ItemId}' was higher than actual amount on cart and '0' amount has remained, and cart item '{si.ItemId}' was removed from cart!";
                }
                else 
                {
                    removedCartItem.Amount = si.Amount;
                }

                removedCartItem.ItemId = itemInCart.ItemId;
                removedCartItem.CartId = cart.CartId;
                removedCartItems.Add(removedCartItem);
            }

            return _resultFact.Result(removedCartItems.AsEnumerable(), true, message);
        }



        public async Task<IServiceResult<int>> AddAmountToStock(int itemId, int amount)
        {
            if (itemId < 1 || amount < 1)
                return _resultFact.Result(0, false, $"Item Id and Amount was not provided !");

            return await _httpInventoryService.AddAmountToStock(itemId, amount);
        }


        public async Task<IServiceResult<int>> RemoveAmountFromStock(int itemId, int amount)
        {
            if (itemId < 1 || amount < 1)
                return _resultFact.Result(0, false, $"Item Id and Amount was not provided !");

            return await _httpInventoryService.RemoveAmountFromStock(itemId, amount);
        }



        public async Task<IServiceResult<IEnumerable<int>>> CartItemsLock(Guid cartId, IEnumerable<int> itemsIds)
        {
            if (cartId == null || cartId == Guid.Empty)
                return _resultFact.Result<IEnumerable<int>>(null, false, $"Cart Id was NOT provided !");
            if (itemsIds == null || !itemsIds.Any())
                return _resultFact.Result<IEnumerable<int>>(null, false, $"Ids for items to lock were NOT provided !");


            var cartItemsToLock = new CartItemsLockCreateDTO { CartId = cartId, ItemsIds = itemsIds};

            var cartItemsLockResult = await _httpSchedulerService.CartItemsLock(cartItemsToLock);

            if(!cartItemsLockResult.Status)
                return _resultFact.Result<IEnumerable<int>>(null, false, cartItemsLockResult.Message);

            return _resultFact.Result(cartItemsLockResult.Data.ItemsIds, true);
        }




        public async Task<IServiceResult<IEnumerable<int>>> CartItemsUnLock(Guid cartId, IEnumerable<int> itemsIds)
        {
            if (cartId == null || cartId == Guid.Empty)
                return _resultFact.Result<IEnumerable<int>>(null, false, $"Cart Id was NOT provided !");
            if (itemsIds == null || !itemsIds.Any())
                return _resultFact.Result<IEnumerable<int>>(null, false, $"Ids for items to unlock were NOT provided !");


            var cartItemsToUnLock = new CartItemsLockDeleteDTO { CartId = cartId, ItemsIds = itemsIds };

            var cartItemsUnlockResult = await _httpSchedulerService.CartItemsUnLock(cartItemsToUnLock);

            if (!cartItemsUnlockResult.Status)
                return _resultFact.Result<IEnumerable<int>>(null, false, cartItemsUnlockResult.Message);

            return _resultFact.Result(cartItemsUnlockResult.Data.ItemsIds, true);
        }




        public IServiceResult<string> CreateOrderCode(int cartId)
        { 
            if(cartId < 1)
                return _resultFact.Result("", false, $"Cart Id must be positive integer larger than 0 !");

            var data = $"User{cartId}-" +
                $"D" +
                $"{DateTime.Now.Day.ToString("00")}" +
                $"{DateTime.Now.Month.ToString("00")}" +
                $"{DateTime.Now.Year.ToString("0000")}" +
                $"T" +
                $"{DateTime.Now.Hour.ToString("00")}" +
                $"{DateTime.Now.Minute.ToString("00")}" +
                $"{DateTime.Now.Second.ToString("00")}";

            var encodedResult = $"OR/{Convert.ToBase64String(Encoding.UTF8.GetBytes(data))}";

            return _resultFact.Result(encodedResult, true);
        }


        public IServiceResult<string> DecodeOrderCode(string orderId)
        { 
            if(string.IsNullOrWhiteSpace(orderId))
                return _resultFact.Result("", false, $"Order code was not provided !");

            var labelLength = 3;

            var data = orderId.Substring(labelLength, orderId.Length - labelLength);

            var resultBytearray = Convert.FromBase64String(data);

            var result = Encoding.ASCII.GetString(resultBytearray);

            return _resultFact.Result(result, true);
        }


    }
}
