using Business.Libraries.ServiceResult.Interfaces;
using Business.Ordering.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Commands.Cart
{
    public class RemoveCartItems_C : IRequest<IServiceResult<IEnumerable<CartItemReadDTO>>>
    {
        public int? UserId { get; set; }
        public IEnumerable<CartItemUpdateDTO> Items { get; set; }


        public class Validator : AbstractValidator<RemoveCartItems_C>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Remove Cart Items command model must NOT be NULL !");
                When(x => x != null, () => {
                    When(x => x.UserId == null, () =>
                    {
                        RuleFor(x => x.Items)
                        .NotNull()
                        .WithMessage("- Items must NOT be NULL !")
                        .NotEmpty()
                        .WithMessage("- Items collection must NOT be empty !");
                        RuleForEach(x => x.Items)
                            .ChildRules(x =>
                            {
                                x.RuleFor(x => x.ItemId)
                                    .GreaterThan(0)
                                    .WithMessage("- Item Id must be greater than 0 !");
                            });
                        RuleForEach(x => x.Items)
                            .ChildRules(x =>
                            {
                                x.RuleFor(x => x.Amount)
                                    .GreaterThan(0)
                                    .WithMessage("- Amount must be greater than 0 !");
                            });
                    });
                    When(x => x.UserId != null, () => {
                        RuleFor(x => x.UserId)
                        .GreaterThan(0)
                        .WithMessage("- User Id must be greater than 0 !");
                    });
                });
            }
        }


        public class RemoveCartItems_H : IRequestHandler<RemoveCartItems_C, IServiceResult<IEnumerable<CartItemReadDTO>>>
        {
            private readonly ICartItemService _cartItemsService;

            public RemoveCartItems_H(ICartItemService cartItemsService)
            {
                _cartItemsService = cartItemsService;
            }


            public async Task<IServiceResult<IEnumerable<CartItemReadDTO>>> Handle(RemoveCartItems_C request, CancellationToken cancellationToken)
            {
                var result = await _cartItemsService.RemoveItemsFromCart(request.UserId ?? 0, request.Items);

                return result;
            }
        }
    }
}
