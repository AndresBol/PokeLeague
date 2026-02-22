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
    public class RepositorySet : IRepositorySet
    {
        private readonly PokeLeagueContext _context;

        public RepositorySet(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<string> AddAsync(Set set)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                await _context.Set<Set>().AddAsync(set);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return set.Id;
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error adding set: {ex.Message}");
            }
        }

        public async Task<Set> FindByIdAsync(string id)
        {
            var set = await _context.Set<Set>()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
            return set!;
        }

        public async Task<ICollection<Set>> ListAsync()
        {
            var sets = await _context.Set<Set>()
                .AsNoTracking()
                .Where(s => s.IsActive)
                .OrderBy(s => s.Id)
                .ToListAsync();
            return sets!;
        }

        public async Task UpdateAsync(Set set)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var existingSet = await _context.Set<Set>()
                    .FirstOrDefaultAsync(s => s.Id == set.Id);

                if (existingSet == null)
                {
                    throw new Exception($"Set with ID {set.Id} not found.");
                }

                existingSet.Name = set.Name;
                existingSet.IsActive = set.IsActive;

                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error updating set: {ex.Message}");
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var set = await _context.Set<Set>().FirstOrDefaultAsync(s => s.Id == id);

                if (set == null)
                {
                    throw new Exception($"Set with ID {id} not found.");
                }

                set.IsActive = false;
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error deleting set: {ex.Message}");
            }
        }
    }
}
