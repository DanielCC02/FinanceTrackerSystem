
using FinanceTracker.Application.Features.Auth.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IJwtTokenService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public LoginHandler(IApplicationDbContext dbContext, IJwtTokenService jwtService, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            var passwordHash = user?.PasswordHash ?? "$2a$11$invalidhashplaceholder";

            var valid = _passwordHasher.VerifyPassword(passwordHash, request.Password);

            if (user == null || !valid || user.IsDeleted)
                throw new KeyNotFoundException("Invalid credentials");

            // Validar email confirmado
            if (!user.EmailConfirmed)
                throw new UnauthorizedAccessException("Please confirm your email before logging in");

            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}
