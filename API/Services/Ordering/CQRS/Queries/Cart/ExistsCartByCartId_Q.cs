using Business.Libraries.ServiceResult.Interfaces;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Queries.Cart
{
    public class ExistsCartByCartId_Q : IRequest<IServiceResult<bool>>
    {
        public Guid CartId { get; set; }


        public class Validator : AbstractValidator<ExistsCartByCartId_Q>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Exists Cart By Cart Id query model must NOT be NULL !");
                When(x => x != null, () => {
                    RuleFor(x => x.CartId)
                        .NotEqual(Guid.Empty)
                        .WithMessage("- Cart Id must not be empty !");
                });
            }
        }


        public class ExistsCartByCartId_H : IRequestHandler<ExistsCartByCartId_Q, IServiceResult<bool>>
        {

            private readonly ICartService _cartService;

            public ExistsCartByCartId_H(ICartService cartService)
            {
                _cartService = cartService;
            }


            public async Task<IServiceResult<bool>> Handle(ExistsCartByCartId_Q request, CancellationToken cancellationToken)
            {
                var result = await _cartService.ExistsCartByCartId(request.CartId);

                return result;
            }
        }
    }
}
