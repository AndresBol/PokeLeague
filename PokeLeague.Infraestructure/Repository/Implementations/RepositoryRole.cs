using Microsoft.EntityFrameworkCore;
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

        public async Task<int> AddAsync(Role role)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                await _context.Set<Role>().AddAsync(role);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return role.Id;
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error adding role: {ex.Message}");
            }
        }

        public async Task<Role> FindByIdAsync(int id)
        {
            var role = await _context.Set<Role>().AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            return role!;
        }

        public async Task<ICollection<Role>> ListAsync()
        {
            var roles = await _context.Set<Role>().AsNoTracking().ToListAsync();
            return roles!;
        }

        public Task UpdateAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
