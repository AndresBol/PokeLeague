using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryAuction
    {
        Task<ICollection<Auction>> ListAsync();
        Task<Auction> FindByIdAsync(int id);
        Task<Auction?> FindActiveByCardIdAsync(int cardId);
        Task<int> AddAsync(Auction auction);
        Task UpdateAsync(Auction auction);
        Task DeleteAsync(int id);
    }
}
