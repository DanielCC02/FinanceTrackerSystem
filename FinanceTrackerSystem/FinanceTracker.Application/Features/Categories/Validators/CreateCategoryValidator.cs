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
                .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters");


            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid category type");
        }
    }
}