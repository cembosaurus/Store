using FluentValidation;

namespace Business.Inventory.DTOs.Item
{
    public class ItemCreateDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? PhotoURL { get; set; }
    }


    public class ItemCreateDTO_V : AbstractValidator<ItemCreateDTO>
    {
        public ItemCreateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Item Create data model must NOT be NULL !");
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

                When(x => !string.IsNullOrWhiteSpace(x.Description), () => {
                    RuleFor(x => x.Description)
                        .MaximumLength(100)
                        .WithMessage("- Description should NOT be longer than 100 characters !");
                });
            });
        }
    }
}
