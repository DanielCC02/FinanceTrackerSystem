
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceTracker.Application.Features.Users.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Queries.GetMyProfile
{
    public class GetMyProfileHandler : IRequestHandler<GetMyProfileQuery, UserDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetMyProfileHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<UserDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Where(u => u.Id == _currentUser.UserId)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException("User not found.");

            return _mapper.Map<UserDto>(user);
        }
    }
}
