using AutoMapper;
using FinanceTracker.Application.Features.Auth.Commands.SendEmailConfirmation;
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
        private readonly IMediator _mediator;

        public CreateUserHandler(IApplicationDbContext dbContext, IPasswordHasher passwordHasher, IMapper mapper, IMediator mediator)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _mediator = mediator;
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

            await _mediator.Send(new SendEmailConfirmationCommand(user.Id, user.Email), cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}
