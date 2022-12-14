using FluentValidation;

namespace Business.Ordering.DTOs
{
    public class CartUpdateDTO
    {
        // Model can be extended,
        // new Cart properties can be updated in the future

        public IEnumerable<CartItemUpdateDTO> Items { get; set; }
    }


    public class CartUpdateDTO_V : AbstractValidator<CartUpdateDTO>
    {
        public CartUpdateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Cart Update data model must NOT be NULL !");
            When(x => x != null, () => {
                RuleFor(x => x.Items)
                    .NotNull()
                    .WithMessage("- Items must NOT be NULL !");
                When(x => x.Items != null, () => {
                    RuleFor(x => x.Items)
                        .NotEmpty()
                        .WithMessage("- Items collection must NOT be empty !");
                    RuleForEach(x => x.Items)
                        .NotNull()
                        .WithMessage("- Cart Items in collection must NOT be NULL !")
                        .ChildRules(x => {
                            x.RuleFor(x => x.ItemId)
                                .GreaterThan(0)
                                .WithMessage("- Item Ids MUST be greater than 0 !");
                        })
                        .ChildRules(x => {
                            x.RuleFor(x => x.Amount)
                                .GreaterThan(0)
                                .WithMessage("- Amount MUST be greater than 0 !");
                        });
                });
            });
        }
    }
}
