using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Cart
{
    public class GetCart_Q : IRequest<IServiceResult<CartReadDTO>>
    {
        public int? UserId { get; set; }


        public class Validator : AbstractValidator<GetCart_Q>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Get Cart query model must NOT be NULL !");
                When(x => x != null, () => {
                    When(x => x.UserId != null, () => {
                        RuleFor(x => x.UserId)
                        .GreaterThan(0)
                        .WithMessage("- User Id must be greater than 0 !");
                    });
                });
            }
        }



        public class GetCart_H : IRequestHandler<GetCart_Q, IServiceResult<CartReadDTO>>
        {
            private readonly ICartService _cartService;

            public GetCart_H(ICartService cartService)
            {
                _cartService = cartService;
            }


            public async Task<IServiceResult<CartReadDTO>> Handle(GetCart_Q request, CancellationToken cancellationToken)
            {
                var result = await _cartService.GetCartByUserId(request.UserId ?? 0);

                return result;
            }
        }
    }
}
