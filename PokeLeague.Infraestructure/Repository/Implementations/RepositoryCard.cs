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
    public class RepositoryCard : IRepositoryCard
    {
        private readonly PokeLeagueContext _context;

        public RepositoryCard(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Card card)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Set<Card>().AddAsync(card);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return card.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error adding card: {ex.Message}");
                }
            });
        }

        public async Task<Card> FindByIdAsync(int id)
        {
            var card = await _context.Set<Card>()
                .AsNoTracking()
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.Set)
                .Include(c => c.Rarity)
                .Include(c => c.LanguageCodeNavigation)
                .Include(c => c.Image)
                .Include(c => c.CategoryCard)
                    .ThenInclude(cc => cc.Category)
                .Include(c => c.Auction)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            return card!;
        }

        public async Task<ICollection<Card>> ListAsync()
        {
            var cards = await _context.Set<Card>()
                .AsNoTracking()
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.Set)
                .Include(c => c.Rarity)
                .Include(c => c.LanguageCodeNavigation)
                .Include(c => c.Image)
                .Include(c => c.CategoryCard)
                    .ThenInclude(cc => cc.Category)
                .Include(c => c.Auction)
                .Where(c => c.IsActive)
                .OrderBy(c => c.Id)
                .ToListAsync();
            return cards!;
        }

        public async Task<ICollection<Card>> ListByUserIdAsync(int userId)
        {
            var cards = await _context.Set<Card>()
                .AsNoTracking()
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.Set)
                .Include(c => c.Rarity)
                .Include(c => c.LanguageCodeNavigation)
                .Include(c => c.Image)
                .Include(c => c.CategoryCard)
                    .ThenInclude(cc => cc.Category)
                .Include(c => c.Auction)
                .Where(c => c.IsActive && c.UserId == userId)
                .OrderBy(c => c.Id)
                .ToListAsync();
            return cards!;
        }

        public async Task UpdateAsync(Card card)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existingCard = await _context.Set<Card>()
                        .FirstOrDefaultAsync(c => c.Id == card.Id);

                    if (existingCard == null)
                    {
                        throw new Exception($"Card with ID {card.Id} not found.");
                    }

                    existingCard.UserId = card.UserId;
                    existingCard.SetId = card.SetId;
                    existingCard.RarityId = card.RarityId;
                    existingCard.LanguageCode = card.LanguageCode;
                    existingCard.Name = card.Name;
                    existingCard.Description = card.Description;
                    existingCard.Grade = card.Grade;
                    existingCard.IsNew = card.IsNew;
                    existingCard.RegistrationDate = card.RegistrationDate;
                    existingCard.IsActive = card.IsActive;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error updating card: {ex.Message}");
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
                    var card = await _context.Set<Card>().FirstOrDefaultAsync(c => c.Id == id);

                    if (card == null)
                    {
                        throw new Exception($"Card with ID {id} not found.");
                    }

                    card.IsActive = false;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error deleting card: {ex.Message}");
                }
            });
        }
    }
}
