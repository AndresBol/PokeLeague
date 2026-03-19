using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>();

            CreateMap<CategoryDTO, Category>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.Name, orig => orig.MapFrom(o => o.Name))
                .ForMember(dest => dest.IsActive, orig => orig.MapFrom(o => o.IsActive))
                .ForMember(dest => dest.CategoryCard, opt => opt.Ignore());
        }
    }
}
