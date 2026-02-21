using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Application.Profiles
{
    public class PurchaseOrderProfile : Profile
    {
        public PurchaseOrderProfile()
        {
            CreateMap<PurchaseOrderDTO, PurchaseOrder>().ReverseMap();

            CreateMap<PurchaseOrderDTO, PurchaseOrder>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.AuctionId, orig => orig.MapFrom(o => o.AuctionId))
                .ForMember(dest => dest.UserId, orig => orig.MapFrom(o => o.UserId))
                .ForMember(dest => dest.PurchaseAmount, orig => orig.MapFrom(o => o.PurchaseAmount))
                .ForMember(dest => dest.IsPaid, orig => orig.MapFrom(o => o.IsPaid))
                .ForMember(dest => dest.IsActive, orig => orig.MapFrom(o => o.IsActive));
        }
    }
}
