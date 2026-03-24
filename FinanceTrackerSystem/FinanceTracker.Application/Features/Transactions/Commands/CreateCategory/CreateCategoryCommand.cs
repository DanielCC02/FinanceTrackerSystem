using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Commands.CreateCategory
{
    public record CreateCategoryCommand
    (
        Guid UserId,
        string Name,
        CategoryType Type
    ) : IRequest<Guid>;
}
