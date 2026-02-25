using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Application.Profiles
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<CardDTO, Card>().ReverseMap();

            CreateMap<CardDTO, Card>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.UserId, orig => orig.MapFrom(o => o.UserId))
                .ForMember(dest => dest.SetId, orig => orig.MapFrom(o => o.SetId))
                .ForMember(dest => dest.RarityId, orig => orig.MapFrom(o => o.RarityId))
                .ForMember(dest => dest.LanguageCode, orig => orig.MapFrom(o => o.LanguageCode))
                .ForMember(dest => dest.Name, orig => orig.MapFrom(o => o.Name))
                .ForMember(dest => dest.Description, orig => orig.MapFrom(o => o.Description))
                .ForMember(dest => dest.Grade, orig => orig.MapFrom(o => o.Grade))
                .ForMember(dest => dest.IsNew, orig => orig.MapFrom(o => o.IsNew))
                .ForMember(dest => dest.RegistrationDate, orig => orig.MapFrom(o => o.RegistrationDate))
                .ForMember(dest => dest.IsActive, orig => orig.MapFrom(o => o.IsActive))
                .ForMember(dest => dest.Set, orig => orig.MapFrom(o => o.Set))
                .ForMember(dest => dest.Rarity, orig => orig.MapFrom(o => o.Rarity))
                .ForMember(dest => dest.LanguageCodeNavigation, orig => orig.MapFrom(o => o.Language))
                .ForMember(dest => dest.Image, orig => orig.MapFrom(o => o.Image))
                .ForMember(dest => dest.CategoryCard, orig => orig.MapFrom(o => o.CategoryCard));

            CreateMap<Card, CardDTO>()
                .ForMember(dest => dest.Language, orig => orig.MapFrom(o => o.LanguageCodeNavigation));
        }
    }
}
