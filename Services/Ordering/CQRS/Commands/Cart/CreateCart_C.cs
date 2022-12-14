using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Commands.Cart
{
    public class CreateCart_C : IRequest<IServiceResult<CartReadDTO>>
    {
        public int UserId { get; set; }


        public class CreateCart_H : IRequestHandler<CreateCart_C, IServiceResult<CartReadDTO>>
        {

            private readonly ICartService _cartService;


            public CreateCart_H(ICartService cartService)
            {
                _cartService = cartService;
            }


            public async Task<IServiceResult<CartReadDTO>> Handle(CreateCart_C request, CancellationToken cancellationToken)
            {
                var result = await _cartService.CreateCart(request.UserId);

                return result;
            }
        }
    }
}
