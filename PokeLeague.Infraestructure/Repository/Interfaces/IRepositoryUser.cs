using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUser
    {
        Task<ICollection<User>> ListAsync();
        Task<User> FindByIdAsync(int id);
        Task<int> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id); 
    }
}
