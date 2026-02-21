using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryRarity
    {
        Task<ICollection<Rarity>> ListAsync();
        Task<Rarity> FindByIdAsync(string id);
        Task<string> AddAsync(Rarity rarity);
        Task UpdateAsync(Rarity rarity);
        Task DeleteAsync(string id); 
    }
}
