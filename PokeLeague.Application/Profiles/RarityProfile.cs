using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Application.Profiles
{
    public class RarityProfile : Profile
    {
        public RarityProfile()
        {
            CreateMap<Rarity, RarityDTO>();

            CreateMap<RarityDTO, Rarity>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.Name, orig => orig.MapFrom(o => o.Name))
                .ForMember(dest => dest.SortOrder, orig => orig.MapFrom(o => o.SortOrder))
                .ForMember(dest => dest.IsActive, orig => orig.MapFrom(o => o.IsActive))
                .ForMember(dest => dest.Card, opt => opt.Ignore());
        }
    }
}
