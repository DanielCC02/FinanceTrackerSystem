using FinanceTracker.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FinanceTracker.Infrastructure.Service.Security.RoleSecurity
{
    public class CurrentUserService : ICurrentUserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId =>
            Guid.TryParse(
                _httpContextAccessor.HttpContext?.User?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value,
                out var userId)
            ? userId
            : throw new UnauthorizedAccessException("User not authenticated");

        public string? Role =>
            _httpContextAccessor.HttpContext?
            .User
            .FindFirst(ClaimTypes.Role)?
            .Value;
    }
}
