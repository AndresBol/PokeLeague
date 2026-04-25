using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCard
    {
        Task<ICollection<Card>> ListAsync();
        Task<Card> FindByIdAsync(int id);
        Task<ICollection<Card>> ListByUserIdAsync(int userId);
        Task<int> AddAsync(Card card);
        Task UpdateAsync(Card card);
        Task DeleteAsync(int id);
    }
}
