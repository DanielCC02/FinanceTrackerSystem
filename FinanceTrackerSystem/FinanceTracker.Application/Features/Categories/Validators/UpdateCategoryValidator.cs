using FluentValidation;
using FinanceTracker.Application.Features.Categories.Commands.UpdateCategory;

namespace FinanceTracker.Application.Features.Categories.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Category ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid category type");
        }
    }
}