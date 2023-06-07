using FluentValidation;

namespace Business.Identity.Models
{
    public class SearchAddressModel
    {
        public int? UserId { get; set; }
        public int? AddressId { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? Number { get; set; }
    }


    public class SearchAddressModel_V : AbstractValidator<SearchAddressModel>
    {
        public SearchAddressModel_V()
        {
            RuleFor(x => x)
                .Must(x => x.UserId.HasValue || x.AddressId.HasValue || !string.IsNullOrWhiteSpace(x.City) || !string.IsNullOrWhiteSpace(x.Street) || x.Number.HasValue)
                .WithMessage("- At least one property is required in Address search model !");

            When(x => x.UserId.HasValue, () =>
            {
                RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("- User Id must be greater than 0 !");
            });

            When(x => x.AddressId.HasValue, () =>
            {
                RuleFor(x => x.AddressId)
                    .GreaterThan(0)
                    .WithMessage("- Address Id must be greater than 0 !");
            });

            When(x => !string.IsNullOrWhiteSpace(x.City), () =>
            {
                RuleFor(x => x.City)
                    .MinimumLength(2)
                    .MaximumLength(30)
                    .WithMessage("- City length should be between 2 - 30 chartacters !");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Street), () =>
            {
                RuleFor(x => x.Street)
                    .MinimumLength(2)
                    .MaximumLength(30)
                    .WithMessage("- Street length should be between 2 - 30 chartacters !");
            });

            When(x => x.Number.HasValue, () =>
            {
                RuleFor(x => x.Number)
                    .GreaterThan(0)
                    .WithMessage("- Street Number must be greater than 0 !");
            });
        }
    }
}
