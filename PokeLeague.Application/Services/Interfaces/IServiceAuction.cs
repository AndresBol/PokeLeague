using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceAuction
    {
        Task<ICollection<AuctionDTO>> ListAsync();
        Task<AuctionDTO> FindByIdAsync(int id);
        Task<int> AddAsync(AuctionDTO auctionDto);
        Task UpdateAsync(AuctionDTO auctionDto);
        Task DeleteAsync(int id);
        Task<AuctionDTO?> FindActiveByCardIdAsync(int cardId);
    }
}
