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
    public class RepositoryCategoryCard : IRepositoryCategoryCard
    {
        private readonly PokeLeagueContext _context;

        public RepositoryCategoryCard(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(CategoryCard categoryCard)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                await _context.Set<CategoryCard>().AddAsync(categoryCard);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return categoryCard.Id;
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error adding category card: {ex.Message}");
            }
        }

        public async Task<CategoryCard> FindByIdAsync(int id)
        {
            var categoryCard = await _context.Set<CategoryCard>()
                .AsNoTracking()
                .Include(cc => cc.Category)
                .FirstOrDefaultAsync(cc => cc.Id == id && cc.IsActive);
            return categoryCard!;
        }

        public async Task<ICollection<CategoryCard>> ListAsync()
        {
            var categoryCards = await _context.Set<CategoryCard>()
                .AsNoTracking()
                .Include(cc => cc.Card)
                    .ThenInclude(c => c.Set)
                .Include(cc => cc.Card)
                    .ThenInclude(c => c.Rarity)
                .Include(cc => cc.Card)
                    .ThenInclude(c => c.LanguageCodeNavigation)
                .Include(cc => cc.Card)
                    .ThenInclude(c => c.User)
                .Include(cc => cc.Category)
                .Where(cc => cc.IsActive)
                .OrderBy(cc => cc.Id)
                .ToListAsync();
            return categoryCards!;
        }

        public async Task UpdateAsync(CategoryCard categoryCard)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var existingCategoryCard = await _context.Set<CategoryCard>()
                    .FirstOrDefaultAsync(cc => cc.Id == categoryCard.Id);

                if (existingCategoryCard == null)
                {
                    throw new Exception($"CategoryCard with ID {categoryCard.Id} not found.");
                }

                existingCategoryCard.CardId = categoryCard.CardId;
                existingCategoryCard.CategoryId = categoryCard.CategoryId;
                existingCategoryCard.IsActive = categoryCard.IsActive;

                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error updating category card: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var categoryCard = await _context.Set<CategoryCard>().FirstOrDefaultAsync(cc => cc.Id == id);

                if (categoryCard == null)
                {
                    throw new Exception($"CategoryCard with ID {id} not found.");
                }

                categoryCard.IsActive = false;
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error deleting category card: {ex.Message}");
            }
        }
    }
}
