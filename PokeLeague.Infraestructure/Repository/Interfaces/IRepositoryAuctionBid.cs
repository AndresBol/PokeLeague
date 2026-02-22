using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryAuctionBid
    {
        Task<ICollection<AuctionBid>> ListAsync();
        Task<AuctionBid> FindByIdAsync(int id);
        Task<int> AddAsync(AuctionBid auctionBid);
        Task UpdateAsync(AuctionBid auctionBid);
        Task DeleteAsync(int id);
    }
}
