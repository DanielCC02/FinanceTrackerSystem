using FluentValidation;
using FinanceTracker.Application.Features.Categories.Commands.UpdateCategory;

namespace FinanceTracker.Application.Features.Categories.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Type)
                .IsInEnum();

            RuleFor(x => x.Icon)
                .MaximumLength(50)
                .When(x => x.Icon != null);
        }
    }
}