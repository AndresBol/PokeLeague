using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceSet
    {
        Task<ICollection<SetDTO>> ListAsync();
        Task<SetDTO> FindByIdAsync(string id);
        Task<string> AddAsync(SetDTO setDto);
        Task UpdateAsync(SetDTO setDto);
        Task DeleteAsync(string id);
    }
}
