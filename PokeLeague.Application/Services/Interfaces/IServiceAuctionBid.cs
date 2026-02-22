using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceAuctionBid
    {
        Task<ICollection<AuctionBidDTO>> ListAsync();
        Task<AuctionBidDTO> FindByIdAsync(int id);
        Task<int> AddAsync(AuctionBidDTO auctionBidDto);
        Task UpdateAsync(AuctionBidDTO auctionBidDto);
        Task DeleteAsync(int id);
    }
}
