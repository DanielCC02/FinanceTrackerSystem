using AutoMapper;
using FinanceTracker.Application.Features.Users.DTOs;
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
