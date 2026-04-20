using FinanceTracker.Application.Features.Categories.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Features.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand
    (
        string Name,
        CategoryType Type,
        string? Icon

    ) : IRequest<CategoryDto>;
}
