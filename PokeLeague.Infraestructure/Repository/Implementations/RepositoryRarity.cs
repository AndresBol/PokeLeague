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
    public class RepositoryRarity : IRepositoryRarity
    {
        private readonly PokeLeagueContext _context;

        public RepositoryRarity(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<string> AddAsync(Rarity rarity)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                await _context.Set<Rarity>().AddAsync(rarity);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return rarity.Id;
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error adding rarity: {ex.Message}");
            }
        }

        public async Task<Rarity> FindByIdAsync(string id)
        {
            var rarity = await _context.Set<Rarity>().AsNoTracking().FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
            return rarity!;
        }

        public async Task<ICollection<Rarity>> ListAsync()
        {
            var rarities = await _context.Set<Rarity>()
                .AsNoTracking()
                .Where(r => r.IsActive)
                .OrderBy(r => r.Id)
                .ToListAsync();

            return rarities!;
        }

        public async Task UpdateAsync(Rarity rarity)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var existingRarity = await _context.Set<Rarity>()
                    .FirstOrDefaultAsync(r => r.Id == rarity.Id);

                if (existingRarity == null)
                {
                    throw new Exception($"Rarity with ID {rarity.Id} not found.");
                }

                existingRarity.Name = rarity.Name;
                existingRarity.SortOrder = rarity.SortOrder;
                existingRarity.IsActive = rarity.IsActive;

                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error updating rarity: {ex.Message}");
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var rarity = await _context.Set<Rarity>().FirstOrDefaultAsync(r => r.Id == id);

                if (rarity == null)
                {
                    throw new Exception($"Rarity with ID {id} not found");

                }

                rarity.IsActive = false;
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error deleting rarity: {ex.Message}");
            }

        }
    }
}
