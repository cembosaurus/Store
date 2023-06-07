using FluentValidation;

namespace Business.Scheduler.DTOs
{
    public class CartItemsLockCreateDTO
    {
        public IEnumerable<int> ItemsIds { get; set; }
        public Guid CartId { get; set; }
    }


    public class CartItemsLockCreateDTO_V : AbstractValidator<CartItemsLockCreateDTO>
    {
        public CartItemsLockCreateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Cart Items Lock create data model must NOT be NULL !");
            When(x => x != null, () => {
                RuleFor(x => x.ItemsIds)
                    .NotNull()
                    .WithMessage("- Item Ids collection must NOT be NULL !");
                When(x => x.ItemsIds != null, () =>
                {
                    RuleFor(x => x.ItemsIds)
                        .NotEmpty()
                        .WithMessage("- Item Ids collection must NOT be empty !");
                    RuleForEach(x => x.ItemsIds)
                        .GreaterThan(0)
                        .WithMessage("- Some Item Ids are NOT positive integers !");
                });

                RuleFor(x => x.CartId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("- Cart Id must not be empty !");
            });
        }
    }
}
