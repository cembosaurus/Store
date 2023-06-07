using FluentValidation;

namespace Business.Ordering.DTOs
{
    public class CartItemUpdateDTO
    {
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }


    public class CartItemUpdateDTO_V : AbstractValidator<CartItemUpdateDTO>
    {
        public CartItemUpdateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Cart Item update data model must NOT be NULL !");
            When(x => x != null, () => {
                RuleFor(x => x.ItemId)
                    .GreaterThan(0)
                    .WithMessage("- Item Id must be greater than 0 !");
                RuleFor(x => x.Amount)
                    .GreaterThan(0)
                    .WithMessage("- Amount must be greater than 0 !");
            });
        }
    }
}
