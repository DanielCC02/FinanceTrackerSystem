using AutoMapper;
using FinanceTracker.Application.Features.Users.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IApplicationDbContext _dbContext;

        private readonly IPasswordHasher _passwordHasher;

        private readonly IMapper _mapper;

        public CreateUserHandler(IApplicationDbContext dbContext, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var normalizedEmail = request.Email.Trim().ToLower();

            var emailExists = await _dbContext.Users
                .AnyAsync(u => u.Email == normalizedEmail, cancellationToken);

            if (emailExists)
                throw new InvalidOperationException("Email already exists.");

            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters");

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var user = new User(
                request.FirstName,
                request.LastName, 
                normalizedEmail,
                passwordHash
                );

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}
