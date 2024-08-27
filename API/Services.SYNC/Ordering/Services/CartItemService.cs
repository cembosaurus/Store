using AutoMapper;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using Business.Scheduler.DTOs;
using Ordering.Data.Repositories.Interfaces;
using Ordering.Services.Interfaces;
using Ordering.Tools.Interfaces;
using Services.Ordering.Models;



namespace Ordering.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IHttpCatalogueItemService _httpCatalogueItemService;
        private readonly ICart _cartTools;
        private readonly ICartItemsRepository _cartItemRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IServiceResultFactory _resultFact;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemsRepository cartItemRepo, ICartRepository cartRepo, IServiceResultFactory resultFact, IMapper mapper, ICart cartTools, IHttpCatalogueItemService httpCatalogueItemService)
        {
            _httpCatalogueItemService = httpCatalogueItemService;
            _cartTools = cartTools;
            _cartItemRepo = cartItemRepo;
            _cartRepo = cartRepo;
            _resultFact = resultFact;
            _mapper = mapper;
        }




        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetAllCardItems()
        {
            var message = "";

            var cartItems = await _cartItemRepo.GetAllCardItems();

            if (cartItems == null || !cartItems.Any())
                message = "Cart items NOT found !";

            return _resultFact.Result(_mapper.Map<IEnumerable<CartItemReadDTO>>(cartItems), true, message);
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> GetCartItems(int userId)
        {
            var message = "";

            var cartItems = await _cartItemRepo.GetCartItemsByUserId(userId);

            if(cartItems == null)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"Cart for user '{userId}' NOT found !");

            if (!cartItems.Any())
                message = $"Cart for user '{userId}' does NOT contain any Items !";

            return _resultFact.Result(_mapper.Map<IEnumerable<CartItemReadDTO>>(cartItems), true, message);
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> AddItemsToCart(int userId, IEnumerable<CartItemUpdateDTO> itemsToAdd)              
        {
            var cart = await _cartRepo.GetCartByUserId(userId);

            if (cart == null)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"Cart '{userId}' NOT found !");

            var message = "";


            // ADD cart items:

            var ids = itemsToAdd.Select(i => i.ItemId);

            var catalogueItemsResult = await _httpCatalogueItemService.GetCatalogueItems(ids);

            if (!catalogueItemsResult.Status)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, catalogueItemsResult.Status, $"Items for cart '{cart.UserId}' were NOT verified ! Reason: {catalogueItemsResult.Message}");

            var matchingIds = catalogueItemsResult.Data.Select(i => i.ItemId);

            if(!matchingIds.Any())
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, catalogueItemsResult.Status, $"Provided NO matching items to add to cart '{cart.UserId}'");

            var mismatchingIds = ids.Except(matchingIds);

            if (mismatchingIds.Any())
                message += Environment.NewLine + $" --> Unmatched Ids: '{string.Join(",", mismatchingIds.ToArray())}' --";


            itemsToAdd = itemsToAdd.Where(i => matchingIds.Contains(i.ItemId));

            var cartItems = _mapper.Map<IEnumerable<CartItem>>(itemsToAdd);

            var addItemsToCartResult = await _cartTools.AddItemsToCart(cart, cartItems);

            if (!addItemsToCartResult.Status || _cartItemRepo.SaveChanges() < 1)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, addItemsToCartResult.Status, addItemsToCartResult.Message);


            // REMOVE items from stock:

            foreach (var ci in cartItems)
            {
                var addToStockResult = await _cartTools.RemoveAmountFromStock(ci.ItemId, ci.Amount);

                if (!addToStockResult.Status)
                    message += Environment.NewLine + $"Amount '{ci.Amount}' for item '{ci.ItemId}' was NOT removed from stock ! Reason: {addToStockResult.Message}";
            }


            // UPDATE cart total:

            var updateCartTotal = await _cartTools.UpdateCartTotal(cart);

            if (!updateCartTotal.Status || _cartItemRepo.SaveChanges() < 1)
                message += Environment.NewLine + $"Total for cart '{cart.UserId}' was NOT updated ! Reason: {updateCartTotal.Message}";


            // LOCK cart items:

            // Items are locked by Item ID, Cart ID and DateTime.
            // Locking process doesn't need amount to identify how many items are locked on cart,
            // amount of locked items is already provided by amount of items on cart:

            var cartItemLockResult = await _cartTools.CartItemsLock(cart.CartId, matchingIds);

            if (!cartItemLockResult.Status)
                message += Environment.NewLine + $"Items were added to cart but NOT locked ! Reason: '{cartItemLockResult.Message}'";

            return _resultFact.Result(_mapper.Map<IEnumerable<CartItemReadDTO>>(addItemsToCartResult.Data), addItemsToCartResult.Status, addItemsToCartResult.Message + message);
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> RemoveItemsFromCart(int userId, IEnumerable<CartItemUpdateDTO> itemsToRemove)
        {
            var cart = await _cartRepo.GetCartByUserId(userId);

            if (cart == null)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"Cart '{userId}' NOT found !");
            if (cart.CartItems == null || !cart.CartItems.Any())
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"Cart items for cart '{userId}' NOT found !");


            var message = string.Empty;


            // REMOVE cart items from list:

            var ids = itemsToRemove.Select(i => i.ItemId);

            var catalogueItemsResult = await _httpCatalogueItemService.GetCatalogueItems(ids);

            if (!catalogueItemsResult.Status)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, catalogueItemsResult.Status, $"Items for cart '{cart.UserId}' were NOT verified ! Reason: {catalogueItemsResult.Message}");

            var matchingIds = catalogueItemsResult.Data.Select(i => i.ItemId);

            if (!matchingIds.Any())
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, catalogueItemsResult.Status, $"Provided NO matching items to remove from cart '{cart.UserId}'");

            var mismatchingIds = ids.Except(matchingIds);

            if (mismatchingIds.Any())
                message += Environment.NewLine + $" --> Unmatched Ids: '{string.Join(",", mismatchingIds.ToArray())}' --";

            itemsToRemove = itemsToRemove.Where(i => matchingIds.Contains(i.ItemId));

            var cartItems = _mapper.Map<IEnumerable<CartItem>>(itemsToRemove);

            var removedCartItemsResult = await _cartTools.RemoveItemsFromCart(cart, _mapper.Map<IEnumerable<CartItem>>(itemsToRemove));

            if (!removedCartItemsResult.Status || _cartRepo.SaveChanges() < 1)
                return _resultFact.Result(_mapper.Map<IEnumerable<CartItemReadDTO>>(removedCartItemsResult.Data), false, removedCartItemsResult.Message);

            message += Environment.NewLine + removedCartItemsResult.Message;

            foreach (var ci in removedCartItemsResult.Data)
            {
                var addToStockResult = await _cartTools.AddAmountToStock(ci.ItemId, ci.Amount);

                if (!addToStockResult.Status)
                    message += Environment.NewLine + $"Amount for item '{ci.ItemId}' was NOT restored into stock ! Reason: {addToStockResult.Message}";
            }


            // UPDATE cart total:

            var updateCartTotalResult = await _cartTools.UpdateCartTotal(cart);

            if (!updateCartTotalResult.Status || _cartItemRepo.SaveChanges() < 1)
                message += Environment.NewLine + $"Total in cart '{cart.UserId}' was NOT updated ! Reason: {updateCartTotalResult.Message}";


            // REMOVE cart items (locks) from scheduler tasks:

            var cartItemToUnlockIds = removedCartItemsResult.Data.Select(rci => rci.ItemId).ToList();

            var cartItemLockRemoveResult = await _cartTools.CartItemsUnLock(cart.CartId, cartItemToUnlockIds);

            if (!cartItemLockRemoveResult.Status)
                message += Environment.NewLine + cartItemLockRemoveResult.Message;

            return _resultFact.Result(_mapper.Map<IEnumerable<CartItemReadDTO>>(removedCartItemsResult.Data), true, message);
        }



        public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> DeleteItemsFromCart(int userId, IEnumerable<int> itemIds)
        {
            var cart = await _cartRepo.GetCartByUserId(userId);

            if (cart == null)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"Cart '{userId}' NOT found !");
            if (cart.CartItems == null || !cart.CartItems.Any())
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"Cart '{userId}' doesn't contain items !");


            var message = string.Empty;


            // REMOVE items:

            var itemsToDelete = cart.CartItems.Where(ci => itemIds.Contains(ci.ItemId)).ToList();

            if(!itemsToDelete.Any())
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"NO matching items to delete found in cart '{cart.CartId}' for user '{cart.UserId}'");

            await _cartItemRepo.DeleteCartItems(itemsToDelete);

            var mismatchingIds = itemIds.Except(itemsToDelete.Select(itd => itd.ItemId));

            var saveResult = _cartItemRepo.SaveChanges();

            if (saveResult < 1)
                return _resultFact.Result<IEnumerable<CartItemReadDTO>>(null, false, $"Cart items were NOT removed !");
            else if (saveResult < itemIds.Count())
                message += Environment.NewLine + $" - {itemIds.Count() - saveResult} items were NOT removed from cart '{cart.CartId}' ! Items: '{string.Join(",", mismatchingIds)}'";


            // UPDATE cart total:

            var updateCartTotalResult = await _cartTools.UpdateCartTotal(cart);

            if (!updateCartTotalResult.Status || _cartItemRepo.SaveChanges() < 1)
                message += Environment.NewLine + $"Total in cart '{cart.UserId}' was NOT updated ! Reason: {updateCartTotalResult.Message}";


            // UPDATE item amount in stock:

            foreach (var ci in itemsToDelete)
            {
                var addAmountToStockResult = await _cartTools.AddAmountToStock(ci.ItemId, ci.Amount);

                if (!addAmountToStockResult.Status)
                    message += Environment.NewLine + $"Amount for item '{ci.ItemId}' was NOT restored in stock ! Reson: '{addAmountToStockResult.Message}'";
            }

            return _resultFact.Result(_mapper.Map<IEnumerable<CartItemReadDTO>>(itemsToDelete), true, message);
        }



        public async Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> DeleteExpiredItems(IEnumerable<CartItemsLockDeleteDTO> carts)
        {
            var message = "";

            foreach (var cart in carts)
            {
                if (cart.CartId == Guid.Empty)
                {
                    message += Environment.NewLine + $"Missing cart id ! Items NOT removed: '{string.Join(",", cart.ItemsIds)}'";

                    continue;
                }


                var cartExistsResult = await _cartRepo.ExistsByCartId(cart.CartId);

                if(!cartExistsResult)
                {
                    // possible duplicate cart ids. Same ID will be added to message each time:
                    message += Environment.NewLine + $"Cart: '{cart.CartId}' does NOT exist !";

                    continue;
                }


                if (cart.ItemsIds == null || !cart.ItemsIds.Any())
                {
                    message += Environment.NewLine + $"List of expired items for Cart: '{cart.CartId}' is empty !";

                    continue;
                }


                var userId = await _cartRepo.GetUserIdByCartId(cart.CartId);

                if(userId < 1)
                    return _resultFact.Result<IEnumerable<CartItemsLockReadDTO>>(null, false, $"User related to Cart: '{cart.CartId}' was NOT found !");

                var cartItemsDeleteResult = await DeleteItemsFromCart(userId, cart.ItemsIds);

                message += Environment.NewLine + cartItemsDeleteResult.Message;


                //-------------------------------------------------------------------------------- Without calling 'GetUserIdByCartId()' ----------------------------------------------------------
                //var itemsInCart = await _cartItemRepo.GetCartItemsByCartId(cil.CartId);

                //var itemsToDelete = itemsInCart.Where(iic => cil.ItemsIds.Contains(iic.ItemId)).ToList();

                //if (itemsToDelete.Count() < itemsInCart.Count())
                //    message += Environment.NewLine + $" - {itemsInCart.Count() - itemsToDelete.Count()} items were NOT removed from cart '{cil.CartId}' because they were NOT present in cart !";

                //await _cartItemRepo.DeleteCartItems(itemsToDelete);

                //if (_cartItemRepo.SaveChanges() < 1)
                //    message += Environment.NewLine + $"Items from cart '{cil.CartId}' were NOT removed !";
                //
                // To Do: implement calculate total and back to stock functionality as in 'GetUserIdByCartId()'
                //
                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            }

            return _resultFact.Result<IEnumerable<CartItemsLockReadDTO>>(null, true, message);
        }
    }
}
