using FinanceTracker.Application.Features.Transactions.Commands.UpdateTransaction;
using FluentValidation;

public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Transaction ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid transaction type");

        RuleFor(x => x.Description)
            .MaximumLength(200)
            .WithMessage("Description cannot exceed 200 characters");

        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Date cannot be in the future");

        RuleFor(x => x.CategoryId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("Invalid CategoryId");
    }
}