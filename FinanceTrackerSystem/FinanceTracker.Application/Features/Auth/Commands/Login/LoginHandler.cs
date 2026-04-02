
using FinanceTracker.Application.Features.Auth.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        public readonly IApplicationDbContext _dbContext;
        public readonly IJwtTokenService _jwtService;
        public readonly IPasswordHasher _passwordHasher;

        public LoginHandler(IApplicationDbContext dbContext, IJwtTokenService jwtService, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLower();

            var user = await _dbContext.Users.
                FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
                ?? throw new KeyNotFoundException("Invalid credentials");

            var valid = _passwordHasher.VerifyPassword(user.PasswordHash, request.Password);

            if(!valid)
                throw new KeyNotFoundException("Invalid credentials");

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
