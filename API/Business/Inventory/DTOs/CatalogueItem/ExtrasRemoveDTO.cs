using FluentValidation;

namespace Business.Inventory.DTOs.CatalogueItem
{
    public class ExtrasRemoveDTO
    {
        public IEnumerable<int>? AccessoryIds { get; set; }
        public IEnumerable<int>? SimilarProductIds { get; set; }
    }


    public class ExtrasRemoveDTO_V : AbstractValidator<ExtrasRemoveDTO>
    {
        public ExtrasRemoveDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Extras Remove data model must NOT be NULL !");
            When(x => x != null, () => {
                When(x => x != null, () => {
                    RuleFor(x => x)
                        .Must(x => x.AccessoryIds != null || x.SimilarProductIds != null)
                        .WithMessage("- At least one property is required in Extras Remove model !");

                    When(x => x.AccessoryIds != null, () => {
                        RuleFor(x => x.AccessoryIds)
                            .NotEmpty()
                            .WithMessage("- Accessory Ids collection must NOT be empty !");
                        RuleForEach(x => x.AccessoryIds)
                            .GreaterThan(0)
                            .WithMessage("- Accessory Ids must be greater than 0");
                    });

                    When(x => x.SimilarProductIds != null, () => {
                        RuleFor(x => x.SimilarProductIds)
                            .NotEmpty()
                            .WithMessage("- Similar Products Ids collection must NOT be empty !");
                        RuleForEach(x => x.SimilarProductIds)
                            .GreaterThan(0)
                            .WithMessage("- Similar Products Ids must be greater than 0");
                    });
                });
            });
        }
    }
}
