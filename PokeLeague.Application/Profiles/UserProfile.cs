using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>().ReverseMap();

            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.Username, orig => orig.MapFrom(o => o.Username))
                .ForMember(dest => dest.Email, orig => orig.MapFrom(o => o.Email))
                .ForMember(dest => dest.PasswordHash, orig => orig.MapFrom(o => o.PasswordHash))
                .ForMember(dest => dest.IsBlocked, orig => orig.MapFrom(o => o.IsBlocked))
                .ForMember(dest => dest.SignupDate, orig => orig.MapFrom(o => o.SignupDate))
                .ForMember(dest => dest.IsActive, orig => orig.MapFrom(o => o.IsActive))
                .ForMember(dest => dest.Role, orig => orig.MapFrom(o => o.Role));
        }
    }
}
