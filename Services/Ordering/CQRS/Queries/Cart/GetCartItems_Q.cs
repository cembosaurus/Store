using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Cart
{
    public class GetCartItems_Q : IRequest<IServiceResult<IEnumerable<CartItemReadDTO>>>
    {
        public int? UserId { get; set; }


        public class Validator : AbstractValidator<GetCartItems_Q>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Get Cart Items query model must NOT be NULL !");
                When(x => x != null, () => {
                    When(x => x.UserId != null, () => {
                        RuleFor(x => x.UserId)
                        .GreaterThan(0)
                        .WithMessage("- User Id must be greater than 0 !");
                    });
                });
            }
        }

        public class GetCartItems_H : IRequestHandler<GetCartItems_Q, IServiceResult<IEnumerable<CartItemReadDTO>>>
        {

            private readonly ICartItemService _cartItemsService;

            public GetCartItems_H(ICartItemService cartItemsService)
            {
                _cartItemsService = cartItemsService;
            }
            public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> Handle(GetCartItems_Q request, CancellationToken cancellationToken)
            {
                var result = await _cartItemsService.GetCartItems(request.UserId ?? 0);

                return result;
            }
        }
    }
}
