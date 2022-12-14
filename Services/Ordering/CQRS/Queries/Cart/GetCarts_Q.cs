using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Cart
{
    public class GetCarts_Q : IRequest<IServiceResult<IEnumerable<CartReadDTO>>>
    {
    }


    public class GetCarts_H : IRequestHandler<GetCarts_Q, IServiceResult<IEnumerable<CartReadDTO>>>
    {

        private readonly ICartService _cartService;


        public GetCarts_H(ICartService cartService)
        {
            _cartService = cartService;
        }


        public async Task<IServiceResult<IEnumerable<CartReadDTO>>> Handle(GetCarts_Q request, CancellationToken cancellationToken)
        {
            var result = await _cartService.GetCards();

            return result;
        }
    }
}
