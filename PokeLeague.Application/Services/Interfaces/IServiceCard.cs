using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceCard
    {
        Task<ICollection<CardDTO>> ListAsync();
        Task<CardDTO> FindByIdAsync(int id);
        Task<int> AddAsync(CardDTO cardDto);
        Task UpdateAsync(CardDTO cardDto);
        Task DeleteAsync(int id);
    }
}
