using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Application.Profiles
{
    public class AuctionBidProfile : Profile
    {
        public AuctionBidProfile()
        {
            CreateMap<AuctionBid, AuctionBidDTO>();

            CreateMap<AuctionBidDTO, AuctionBid>()
                .ForMember(dest => dest.Id, orig => orig.MapFrom(o => o.Id))
                .ForMember(dest => dest.AuctionId, orig => orig.MapFrom(o => o.AuctionId))
                .ForMember(dest => dest.UserId, orig => orig.MapFrom(o => o.UserId))
                .ForMember(dest => dest.BidAmount, orig => orig.MapFrom(o => o.BidAmount))
                .ForMember(dest => dest.BidDate, orig => orig.MapFrom(o => o.BidDate))
                .ForMember(dest => dest.IsActive, orig => orig.MapFrom(o => o.IsActive))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Auction, opt => opt.Ignore());
        }
    }
}
