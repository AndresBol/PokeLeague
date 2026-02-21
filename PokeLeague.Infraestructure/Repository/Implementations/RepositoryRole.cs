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
            var role = await _context.Set<Role>()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
            return role!;
        }

        public async Task<ICollection<Role>> ListAsync()
        {
            var roles = await _context.Set<Role>()
                .AsNoTracking()
                .Where(r => r.IsActive)
                .OrderBy(r => r.Id)
                .ToListAsync();
            return roles!;
        }

        public async Task UpdateAsync(Role role)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                
                var existingRole = await _context.Set<Role>()
                    .FirstOrDefaultAsync(r => r.Id == role.Id);
                
                if (existingRole == null)
                {
                    throw new Exception($"Role with ID {role.Id} not found.");
                }

                existingRole.Name = role.Name;
                existingRole.IsActive = role.IsActive;
                
                await _context.SaveChangesAsync();
                
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error updating role: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var role = await _context.Set<Role>().FirstOrDefaultAsync(r => r.Id == id);
                
                if (role == null)
                {
                    throw new Exception($"Role with ID {id} not found.");
                }

                role.IsActive = false;
                await _context.SaveChangesAsync();
                
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error deleting role: {ex.Message}");
            }
        }
    }
}
