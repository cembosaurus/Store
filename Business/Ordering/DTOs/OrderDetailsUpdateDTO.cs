using FluentValidation;

namespace Business.Ordering.DTOs
{
    public class OrderDetailsUpdateDTO
    {
        public string? Name { get; set; }
        public int? AddressId { get; set; }
    }


    public class OrderDetailsUpdateDTO_V : AbstractValidator<OrderDetailsUpdateDTO>
    {
        public OrderDetailsUpdateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Order Details update data model must NOT be NULL !");
            When(x => x != null, () => {
                RuleFor(x => x)
                    .Must(x => !string.IsNullOrWhiteSpace(x.Name) || x.AddressId.HasValue)
                    .WithMessage("- At least one property is required for Order Details update data model !");

                When(x => x != null, () => {
                    When(x => !string.IsNullOrWhiteSpace(x.Name), () => {
                        RuleFor(x => x.Name)
                            .MinimumLength(5)
                            .MaximumLength(30)
                            .WithMessage("- Name length should be between 5 - 30 chartacters !");
                    });

                    When(x => x.AddressId.HasValue, () => {
                        RuleFor(x => x.AddressId)
                            .GreaterThan(0)
                            .WithMessage("- Address Id must be greater than 0 !");
                    });
                });
            });
        }
    }
}
