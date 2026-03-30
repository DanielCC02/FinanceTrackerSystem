using FinanceTracker.Application.Features.Categories.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery
    (
        Guid Id
    ) : IRequest<CategoryDto>;
}
