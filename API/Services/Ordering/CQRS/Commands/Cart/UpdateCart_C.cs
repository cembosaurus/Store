using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Commands.Cart
{
    public class UpdateCart_C : IRequest<IServiceResult<CartReadDTO>>
    {
        public int? UserId { get; set; }
        public CartUpdateDTO CartUpdateDTO { get; set; }

        public class Validator : AbstractValidator<UpdateCart_C>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Update Cart command model must NOT be NULL !");
                When(x => x != null, () => {
                    When(x => x.UserId == null, () =>
                    {
                        RuleFor(x => x.CartUpdateDTO)
                            .NotNull()
                            .WithMessage("- Cart Update data must NOT be NULL !");
                        When(x => x.CartUpdateDTO != null, () =>
                        {
                            RuleFor(x => x.CartUpdateDTO.Items)
                            .NotNull()
                            .WithMessage("- Items must NOT be NULL !")
                            .NotEmpty()
                            .WithMessage("- Items collection must NOT be empty !");
                            RuleForEach(x => x.CartUpdateDTO.Items)
                                .ChildRules(x =>
                                {
                                    x.RuleFor(x => x.ItemId)
                                        .GreaterThan(0)
                                        .WithMessage("- Item Id must be greater than 0 !");
                                });
                            RuleForEach(x => x.CartUpdateDTO.Items)
                                .ChildRules(x =>
                                {
                                    x.RuleFor(x => x.Amount)
                                        .GreaterThan(0)
                                        .WithMessage("- Amount must be greater than 0 !");
                                });
                        });
                    });
                    When(x => x.UserId != null, () =>
                    {
                        RuleFor(x => x.UserId)
                        .GreaterThan(0)
                        .WithMessage("- User Id must be greater than 0 !");
                    });
                });
            }
        }


        public class UpdateCart_H : IRequestHandler<UpdateCart_C, IServiceResult<CartReadDTO>>
        {
            private readonly ICartService _cartService;

            public UpdateCart_H(ICartService cartService)
            {
                _cartService = cartService;
            }


            public async Task<IServiceResult<CartReadDTO>> Handle(UpdateCart_C request, CancellationToken cancellationToken)
            {
                var result = await _cartService.UpdateCart(request.UserId ?? 0, request.CartUpdateDTO);

                return result;
            }
        }
    }
}
