using FinanceTracker.Application.Features.Transactions.Commands.UpdateTransaction;
using FluentValidation;

public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Type)
            .IsInEnum();

        RuleFor(x => x.Description)
            .MaximumLength(200)
            .When(x => x.Description != null);

        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateTime.UtcNow);
    }
}