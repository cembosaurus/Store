using FluentValidation;

namespace Business.Ordering.DTOs
{
    public class OrderCreateDTO
    {
        public OrderDetailsCreateDTO OrderDetails { get; set; }
    }


    public class OrderCreateDTO_V : AbstractValidator<OrderCreateDTO>
    {
        public OrderCreateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Order Create data model must NOT be NULL !");
            When(x => x != null, () => {
                RuleFor(x => x.OrderDetails)
                    .NotNull()
                    .WithMessage("- Order Details must NOT be NULL !");

                When(x => x.OrderDetails != null, () => {
                    RuleFor(x => x.OrderDetails.Name)
                        .NotNull()
                        .WithMessage("- Name must NOT be NULL !");
                    When(x => !string.IsNullOrWhiteSpace(x.OrderDetails.Name), () => {
                        RuleFor(x => x.OrderDetails.Name)
                            .NotEmpty()
                            .WithMessage("- Name must NOT be empty !")
                            .MinimumLength(5)
                            .MaximumLength(30)
                            .WithMessage("- Name length should be between 5 - 30 chartacters !");
                    });

                    RuleFor(x => x.OrderDetails.AddressId)
                        .GreaterThan(0)
                        .WithMessage("- Address Id is NOT in the range !");
                });
            });

        }
    }
}
