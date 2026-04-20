using FluentValidation;
using FinanceTracker.Application.Features.Categories.Commands.CreateCategory;

namespace FinanceTracker.Application.Features.Categories.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(100);

            RuleFor(x => x.Type)
                .IsInEnum();

            RuleFor(x => x.Icon)
                .MaximumLength(50)
                .When(x => x.Icon != null);
        }
    }
}