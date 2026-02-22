using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositorySet
    {
        Task<ICollection<Set>> ListAsync();
        Task<Set> FindByIdAsync(string id);
        Task<string> AddAsync(Set set);
        Task UpdateAsync(Set set);
        Task DeleteAsync(string id);
    }
}
