namespace FinanceTracker.Application.Features.Transactions.Validators;

using FluentValidation;
using FinanceTracker.Application.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionValidator : AbstractValidator<DeleteTransactionCommand>
{
    public DeleteTransactionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Transaction ID is required");
    }
}