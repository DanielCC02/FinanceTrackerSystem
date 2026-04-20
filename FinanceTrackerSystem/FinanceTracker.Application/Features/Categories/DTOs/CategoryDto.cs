
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Features.Categories.DTOs
{
    public record CategoryDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
    }
}
