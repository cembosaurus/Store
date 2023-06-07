using FluentValidation;

namespace Business.Ordering.DTOs
{
    public class OrderDetailsCreateDTO
    {
        public string Name { get; set; }
        public int AddressId { get; set; }
    }

    
    public class OrderDetailsCreateDTO_V : AbstractValidator<OrderDetailsCreateDTO>
    {
        public OrderDetailsCreateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Order Details create data model must NOT be NULL !");
            When(x => x != null, () => {
                RuleFor(x => x.Name)
                    .NotNull()
                    .WithMessage("- Name must NOT be NULL !");
                When(x => !string.IsNullOrWhiteSpace(x.Name), () => {
                    RuleFor(x => x.Name)
                        .NotEmpty()
                        .WithMessage("- Name must NOT be empty !")
                        .MinimumLength(5)
                        .MaximumLength(30)
                        .WithMessage("- Name length should be between 5 - 30 chartacters !");
                });

                RuleFor(x => x.AddressId)
                    .GreaterThan(0)
                    .WithMessage("- Address Id is NOT in the range !");
            });
        }
    }
}
