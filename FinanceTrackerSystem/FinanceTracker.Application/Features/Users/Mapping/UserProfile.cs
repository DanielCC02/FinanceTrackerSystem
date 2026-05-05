using AutoMapper;
using FinanceTracker.Application.Features.Users.DTOs;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Features.Users.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // 🔹 SIMPLE DTO
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            // 🔹 DETAILS DTO
            CreateMap<User, UserDetailsDto>()
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => src.Role.ToString()));
        }
    }
}
