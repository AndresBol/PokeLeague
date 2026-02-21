using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceUser
    {
        Task<ICollection<UserDTO>> ListAsync();
        Task<UserDTO> FindByIdAsync(int id);
        Task<int> AddAsync(UserDTO userDto);
        Task UpdateAsync(UserDTO userDto);
        Task DeleteAsync(int id);
    }
}
