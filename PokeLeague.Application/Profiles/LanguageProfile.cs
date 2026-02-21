using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Application.Profiles
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<LanguageDTO, Language>().ReverseMap();

            CreateMap<LanguageDTO, Language>()
                .ForMember(dest => dest.LanguageCode, orig => orig.MapFrom(o => o.LanguageCode))
                .ForMember(dest => dest.LanguageName, orig => orig.MapFrom(o => o.LanguageName))
                .ForMember(dest => dest.IsActive, orig => orig.MapFrom(o => o.IsActive));
        }
    }
}
