using AutoMapper;
using FinanceTracker.Application.Features.Users.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, UserDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public UpdateProfileHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser, IMapper mapper)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId == Guid.Empty)
                throw new UnauthorizedAccessException("User not authenticated");

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == _currentUser.UserId, cancellationToken)
                ?? throw new Exception("User not found");

            if (user.IsDeleted)
                throw new UnauthorizedAccessException("User is inactive");

            var firstName = request.FirstName.Trim();
            var lastName = request.LastName.Trim();
            var phone = request.PhoneNumber.Trim();

            user.UpdateProfile(firstName, lastName, phone);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}
