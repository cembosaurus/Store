using FluentValidation;

namespace Business.Inventory.DTOs.Item
{
    public class ItemUpdateDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PhotoURL { get; set; }
  }


    public class ItemUpdateDTO_V : AbstractValidator<ItemUpdateDTO>
    {
        public ItemUpdateDTO_V()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("- Item Update data model must NOT be NULL !");
            When(x => x != null, () => {
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
