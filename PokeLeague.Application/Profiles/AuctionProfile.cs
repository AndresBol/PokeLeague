using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Application.Profiles
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            CreateMap<Auction, AuctionDTO>();

            CreateMap<AuctionDTO, Auction>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.UserId, orig => orig.MapFrom(o => o.UserId))
                .ForMember(dest => dest.CardId, orig => orig.MapFrom(o => o.CardId))
                .ForMember(dest => dest.StartDate, orig => orig.MapFrom(o => o.StartDate))
                .ForMember(dest => dest.EndDate, orig => orig.MapFrom(o => o.EndDate))
                .ForMember(dest => dest.BasePrice, orig => orig.MapFrom(o => o.BasePrice))
                .ForMember(dest => dest.MinIncrease, orig => orig.MapFrom(o => o.MinIncrease))
                .ForMember(dest => dest.IsCanceled, orig => orig.MapFrom(o => o.IsCanceled))
                .ForMember(dest => dest.IsActive, orig => orig.MapFrom(o => o.IsActive))
                .ForMember(dest => dest.Card, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseOrder, opt => opt.Ignore())
                .ForMember(dest => dest.AuctionBid, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
