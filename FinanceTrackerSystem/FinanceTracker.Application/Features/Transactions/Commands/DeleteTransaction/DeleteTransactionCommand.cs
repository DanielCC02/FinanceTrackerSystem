using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Commands.DeleteTransaction
{
    public record DeleteTransactionCommand
    (
       Guid AccountId,
       Guid Id
    ) : IRequest<Unit>;
}
