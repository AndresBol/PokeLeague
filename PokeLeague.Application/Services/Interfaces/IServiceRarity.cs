using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceRarity
    {
        Task<ICollection<RarityDTO>> ListAsync();
        Task<RarityDTO> FindByIdAsync(string id);
        Task<string> AddAsync(RarityDTO rarityDto);
        Task UpdateAsync(RarityDTO rarityDto);
        Task DeleteAsync(string id);
    }
}
