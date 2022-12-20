using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.DTOs;
using FluentValidation;
using MediatR;
using Ordering.Services.Interfaces;

namespace Ordering.CQRS.Commands.Cart
{
    public class DeleteExpiredCartItems_C : IRequest<IServiceResult<IEnumerable<CartItemsLockReadDTO>>>
    {
        public IEnumerable<CartItemsLockDeleteDTO> CartItemLocks { get; set; }

        public class Validator : AbstractValidator<DeleteExpiredCartItems_C>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .NotNull()
                    .WithMessage("- Delete Expired Cart Items command model must NOT be NULL !");
                When(x => x != null, () => {
                    RuleFor(x => x.CartItemLocks)
                        .NotNull()
                        .WithMessage("- Cart Item Locks data must NOT be NULL !");
                    When(x => x.CartItemLocks != null, () =>
                    {
                        RuleFor(x => x.CartItemLocks)
                        .NotEmpty()
                        .WithMessage("- Cart Item Locks data must NOT be empty !");
                        RuleForEach(x => x.CartItemLocks)
                        .ChildRules(x => {
                            x.RuleFor(x => x.ItemsIds)
                                .NotNull()
                                .WithMessage($"- Items Ids collection must NOT be NULL !")
                                .NotEmpty()
                                .WithMessage($"- Items Ids collection must NOT be empty !");
                            x.RuleForEach(x => x.ItemsIds)
                                .GreaterThan(0)
                                .WithMessage("- Items Ids must be greater than 0 !");
                            x.RuleFor(x => x.CartId)
                                .NotEqual(Guid.Empty)
                                .WithMessage("- Cart Id must not be empty !");
                        });
                    });
                });
            }
        }


        public class DeleteExpiredCartItems_H : IRequestHandler<DeleteExpiredCartItems_C, IServiceResult<IEnumerable<CartItemsLockReadDTO>>>
        {
            private readonly ICartItemService _cartItemsService;

            public DeleteExpiredCartItems_H(ICartItemService cartItemsService)
            {
                _cartItemsService = cartItemsService;
            }


            public async Task<IServiceResult<IEnumerable<CartItemsLockReadDTO>>> Handle(DeleteExpiredCartItems_C request, CancellationToken cancellationToken)
            {
                var result = await _cartItemsService.DeleteExpiredItems(request.CartItemLocks);

                return result;
            }
        }
    }
}
