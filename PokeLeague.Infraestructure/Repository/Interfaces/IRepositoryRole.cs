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
        Task<Role> FindByNameAsync(string name);
        Task<int> AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(int id); 
    }
}
