using PokeLeague.Infraestructure.Data;
using PokeLeague.Infraestructure.Models;
using PokeLeague.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Implementations
{
    public class RepositoryRole : IRepositoryRole
    {
        private readonly PokeLeagueContext _context;

        public RepositoryRole(PokeLeagueContext context)
        {
            _context = context;
        }

        public Task<int> AddAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Role> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Role>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
