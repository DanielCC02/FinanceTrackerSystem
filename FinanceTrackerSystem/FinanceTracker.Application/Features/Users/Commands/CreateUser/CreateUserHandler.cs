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
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            var firstName = request.FirstName.Trim();
            var lastName = request.LastName.Trim();

            var emailExists = await _dbContext.Users
                .AnyAsync(u => u.Email == normalizedEmail && !u.IsDeleted, cancellationToken);

            if (emailExists)
                throw new Exception("Email already exists.");            

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var user = new User(
                firstName,
                lastName, 
                normalizedEmail,
                passwordHash
                );

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}
