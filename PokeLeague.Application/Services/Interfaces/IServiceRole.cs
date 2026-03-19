using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceRole
    {
        Task<ICollection<RoleDTO>> ListAsync();
        Task<RoleDTO> FindByIdAsync(int id);
        Task<RoleDTO> FindByNameAsync(string name);
        Task<int> AddAsync(RoleDTO roleDto);
        Task UpdateAsync(RoleDTO roleDto);
        Task DeleteAsync(int id);
    }
}
