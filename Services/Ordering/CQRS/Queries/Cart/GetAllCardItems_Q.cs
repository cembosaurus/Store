using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Cart
{
    public class GetAllCardItems_Q : IRequest<IServiceResult<IEnumerable<CartItemReadDTO>>>
    {


        public class GetAllCardItems_H : IRequestHandler<GetAllCardItems_Q, IServiceResult<IEnumerable<CartItemReadDTO>>>
        {

            private readonly ICartItemService _cartItemsService;

            public GetAllCardItems_H(ICartItemService cartItemsService)
            {
                _cartItemsService = cartItemsService;
            }

            public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> Handle(GetAllCardItems_Q request, CancellationToken cancellationToken)
            {
                var result = await _cartItemsService.GetAllCardItems();

                return result;
            }
        }
    }
}
