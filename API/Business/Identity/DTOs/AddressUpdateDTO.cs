using FluentValidation;

namespace Business.Identity.DTOs
{
    public class AddressUpdateDTO
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
    }


    public class AddressUpdateDTO_V : AbstractValidator<AddressUpdateDTO>
    {
        public AddressUpdateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Address update data model must NOT be NULL ! ");
            When(x => x != null, () => {
                RuleFor(x => x.City)
                    .NotNull()
                    .WithMessage("- City must NOT be NULL !");
                When(x => !string.IsNullOrWhiteSpace(x.City), () => {
                    RuleFor(x => x.City)
                        .NotEmpty()
                        .WithMessage("- City must NOT be empty !")
                        .MinimumLength(2)
                        .MaximumLength(30)
                        .WithMessage("- City length should be between 2 - 30 chartacters !");
                });

                RuleFor(x => x.Street)
                    .NotNull()
                    .WithMessage("- Street must NOT be NULL !");
                When(x => !string.IsNullOrWhiteSpace(x.Street), () => {
                    RuleFor(x => x.Street)
                        .NotEmpty()
                        .WithMessage("- Street must NOT be empty !")
                        .MinimumLength(2)
                        .MaximumLength(30)
                        .WithMessage("- Street length should be between 2 - 30 chartacters !");
                });

                RuleFor(x => x.Number)
                    .GreaterThan(0)
                    .WithMessage("- Street must be greater than 0 !");
            });
        }
    }
}
