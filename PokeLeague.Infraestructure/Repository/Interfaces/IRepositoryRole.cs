using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryRole
    {
        Task<ICollection<Role>> ListAsync();
        Task<Role> FindByIdAsync(int id);
        Task<int> AddAsync(Role role);
        Task<int> UpdateAsync(Role role);
        Task<int> DeleteAsync(int id); 
    }
}
