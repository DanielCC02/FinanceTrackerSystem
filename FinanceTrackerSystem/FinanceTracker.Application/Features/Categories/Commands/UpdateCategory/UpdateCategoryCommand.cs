
using FinanceTracker.Application.Features.Categories.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand
    (
        Guid Id,
        string Name,
        CategoryType Type,
        string? Icon
    ) : IRequest<CategoryDto>;
}
