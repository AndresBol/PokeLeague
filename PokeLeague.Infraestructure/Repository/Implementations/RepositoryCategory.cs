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
    public class RepositoryCategory : IRepositoryCategory
    {
        private readonly PokeLeagueContext _context;

        public RepositoryCategory(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Category category)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Set<Category>().AddAsync(category);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return category.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error adding category: {ex.Message}");
                }
            });
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            var category = await _context.Set<Category>()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            return category!;
        }

        public async Task<ICollection<Category>> ListAsync()
        {
            var categories = await _context.Set<Category>()
                .AsNoTracking()
                .Include(c => c.CategoryCard)
                    .ThenInclude(cc => cc.Card)
                .Where(c => c.IsActive)
                .OrderBy(c => c.Id)
                .ToListAsync();
            return categories!;
        }

        public async Task UpdateAsync(Category category)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existingCategory = await _context.Set<Category>()
                        .FirstOrDefaultAsync(c => c.Id == category.Id);

                    if (existingCategory == null)
                    {
                        throw new Exception($"Category with ID {category.Id} not found.");
                    }

                    existingCategory.Name = category.Name;
                    existingCategory.IsActive = category.IsActive;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error updating category: {ex.Message}");
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
                    var category = await _context.Set<Category>().FirstOrDefaultAsync(c => c.Id == id);

                    if (category == null)
                    {
                        throw new Exception($"Category with ID {id} not found.");
                    }

                    category.IsActive = false;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error deleting category: {ex.Message}");
                }
            });
        }
    }
}
