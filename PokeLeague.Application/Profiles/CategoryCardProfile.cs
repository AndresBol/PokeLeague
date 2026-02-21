using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Application.Profiles
{
    public class CategoryCardProfile : Profile
    {
        public CategoryCardProfile()
        {
            CreateMap<CategoryCardDTO, CategoryCard>().ReverseMap();

            CreateMap<CategoryCardDTO, CategoryCard>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.CardId, orig => orig.MapFrom(o => o.CardId))
                .ForMember(dest => dest.CategoryId, orig => orig.MapFrom(o => o.CategoryId))
                .ForMember(dest => dest.IsActive, orig => orig.MapFrom(o => o.IsActive))
                .ForMember(dest => dest.Category, orig => orig.MapFrom(o => o.Category));
        }
    }
}
