using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Application.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<ImageDTO, Image>().ReverseMap();

            CreateMap<ImageDTO, Image>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.CardId, orig => orig.MapFrom(o => o.CardId))
                .ForMember(dest => dest.ImageData, orig => orig.MapFrom(o => o.ImageData))
                .ForMember(dest => dest.IsActive, orig => orig.MapFrom(o => o.IsActive));
        }
    }
}
