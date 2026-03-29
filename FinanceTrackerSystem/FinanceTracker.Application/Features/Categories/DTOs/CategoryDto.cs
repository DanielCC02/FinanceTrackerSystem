
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Features.Categories.DTOs
{
    public record CategoryDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string Name { get; init; } = string.Empty;
        public CategoryType Type { get; init; }
    }
}
