using FinanceTracker.Domain.Enums;

namespace FinanceTracker.API.Requests
{
    public record UpdateTransactionRequest
    (
        Guid? CategoryId,
        decimal Amount,
        TransactionType Type,
        string Description,
        DateTime Date
    );
}
