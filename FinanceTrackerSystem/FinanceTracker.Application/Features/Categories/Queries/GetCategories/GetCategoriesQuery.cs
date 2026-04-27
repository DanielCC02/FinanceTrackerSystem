using FinanceTracker.Application.Features.Categories.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery
    (
        TransactionType? Type
    ) : IRequest<List<CategoryDto>>;
}
