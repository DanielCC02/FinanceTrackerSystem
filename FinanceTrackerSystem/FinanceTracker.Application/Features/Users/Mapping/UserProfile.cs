using AutoMapper;
using FinanceTracker.Application.Features.Users.Commands.CreateUser;
using FinanceTracker.Application.Features.Users.Queries.GetUsers;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Features.Users.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<User, CreateUserResponseDto>();
        }
    }
}
