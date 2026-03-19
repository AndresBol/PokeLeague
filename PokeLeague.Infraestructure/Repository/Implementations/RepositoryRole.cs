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
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Set<Role>().AddAsync(role);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return role.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error adding role: {ex.Message}");
                }
            });
        }

        public async Task<Role> FindByIdAsync(int id)
        {
            var role = await _context.Set<Role>().AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            return role!;
        }
        public async Task<Role> FindByNameAsync(string name)
        {
            var role = await _context.Set<Role>().AsNoTracking().FirstOrDefaultAsync(r => r.Name == name);
            return role!;
        }

        public async Task<ICollection<Role>> ListAsync()
        {
            var roles = await _context.Set<Role>().AsNoTracking().ToListAsync();
            return roles!;
        }

        public async Task UpdateAsync(Role role)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existingRole = await _context.Set<Role>()
                        .FirstOrDefaultAsync(r => r.Id == role.Id);

                    if (existingRole == null)
                    {
                        throw new Exception($"Role with ID {role.Id} not found.");
                    }

                    existingRole.Name = role.Name;
                    existingRole.IsActive = role.IsActive;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error updating role: {ex.Message}");
                }
            });
        }

        public async Task DeleteAsync(int id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var role = await _context.Set<Role>().FirstOrDefaultAsync(r => r.Id == id);

                    if (role == null)
                    {
                        throw new Exception($"Role with ID {id} not found.");
                    }

                    role.IsActive = false;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error deleting role: {ex.Message}");
                }
            });
        }
    }
}
