using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Commands.Cart
{
    public class DeleteCart_C : IRequest<IServiceResult<CartReadDTO>>
    {
        public int? UserId { get; set; }


        public class Validator : AbstractValidator<DeleteCart_C>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Delete Cart command model must NOT be NULL !");
                When(x => x != null, () => {
                    When(x => x.UserId != null, () =>
                    {
                        RuleFor(x => x.UserId)
                        .GreaterThan(0)
                        .WithMessage("- User Id must be greater than 0 !");
                    });
                });
            }


            public class DeleteCart_H : IRequestHandler<DeleteCart_C, IServiceResult<CartReadDTO>>
            {
                private readonly ICartService _cartService;

                public DeleteCart_H(ICartService cartService)
                {
                    _cartService = cartService;
                }


                public async Task<IServiceResult<CartReadDTO>> Handle(DeleteCart_C request, CancellationToken cancellationToken)
                {
                    var result = await _cartService.DeleteCart(request.UserId ?? 0);

                    return result;
                }
            }
        }
    }
}
