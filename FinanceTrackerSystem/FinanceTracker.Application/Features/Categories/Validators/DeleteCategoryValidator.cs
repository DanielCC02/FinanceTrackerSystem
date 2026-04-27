using FluentValidation;
using FinanceTracker.Application.Features.Categories.Commands.DeleteCategory;

namespace FinanceTracker.Application.Features.Categories.Validators
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Category ID is required");
        }
    }
}