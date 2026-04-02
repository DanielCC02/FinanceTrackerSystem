using FinanceTracker.Application.Features.Auth.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.Login
{
    public record LoginCommand
    (
        string Email,
        string Password
    ) : IRequest<LoginResponseDto>;
}
