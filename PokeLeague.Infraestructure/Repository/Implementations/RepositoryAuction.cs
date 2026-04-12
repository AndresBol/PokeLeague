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
    public class RepositoryAuction : IRepositoryAuction
    {
        private readonly PokeLeagueContext _context;

        public RepositoryAuction(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Auction auction)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Set<Auction>().AddAsync(auction);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return auction.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error adding auction: {ex.Message}");
                }
            });
        }

        public async Task<Auction?> FindActiveByCardIdAsync(int cardId)
        {
            var now = DateTime.Now;
            var auction = await _context.Set<Auction>()
                .AsNoTracking()
                .Where(a => a.CardId == cardId && a.IsActive && !a.IsCanceled && a.EndDate >= now)
                .OrderByDescending(a => a.StartDate)
                .FirstOrDefaultAsync();
            return auction;
        }

        public async Task<Auction> FindByIdAsync(int id)
        {
            var auction = await _context.Set<Auction>()
                .AsNoTracking()
                .Include(a => a.Card)
                .ThenInclude(c => c.Image)
                .Include(a => a.Card)
                .ThenInclude(c => c.CategoryCard)
                .ThenInclude(cc => cc.Category)
                .Include(a => a.AuctionBid)
                    .ThenInclude(ab => ab.User)
                .Include(a => a.PurchaseOrder)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
            return auction!;
        }

        public async Task<ICollection<Auction>> ListAsync()
        {
            var auctions = await _context.Set<Auction>()
                .AsNoTracking()
                .Include(a => a.User)
                    .ThenInclude(u => u.Role)
                .Include(a => a.Card)
                    .ThenInclude(c => c.Set)
                .Include(a => a.Card)
                    .ThenInclude(c => c.Rarity)
                .Include(a => a.Card)
                    .ThenInclude(c => c.LanguageCodeNavigation)
                .Include(a => a.Card)
                    .ThenInclude(c => c.User)
                .Include(a => a.Card)
                    .ThenInclude(c => c.Image)
                .Include(a => a.AuctionBid)
                    .ThenInclude(ab => ab.User)
                .Include(a => a.PurchaseOrder)
                    .ThenInclude(po => po.User)
                .Where(a => a.IsActive)
                .OrderBy(a => a.Id)
                .ToListAsync();
            return auctions!;
        }

        public async Task UpdateAsync(Auction auction)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existingAuction = await _context.Set<Auction>()
                        .FirstOrDefaultAsync(a => a.Id == auction.Id);

                    if (existingAuction == null)
                    {
                        throw new Exception($"Auction with ID {auction.Id} not found.");
                    }

                    existingAuction.UserId = auction.UserId;
                    existingAuction.CardId = auction.CardId;
                    existingAuction.StartDate = auction.StartDate;
                    existingAuction.EndDate = auction.EndDate;
                    existingAuction.BasePrice = auction.BasePrice;
                    existingAuction.MinIncrease = auction.MinIncrease;
                    existingAuction.IsCanceled = auction.IsCanceled;
                    existingAuction.IsActive = auction.IsActive;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error updating auction: {ex.Message}");
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
                    var auction = await _context.Set<Auction>().FirstOrDefaultAsync(a => a.Id == id);

                    if (auction == null)
                    {
                        throw new Exception($"Auction with ID {id} not found.");
                    }

                    auction.IsActive = false;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error deleting auction: {ex.Message}");
                }
            });
        }

        public async Task<Auction?> GetAuctionWithBids(int id) 
        {
            return await _context.Set<Auction>()
                .Include(a => a.AuctionBid)
                .ThenInclude(ab => ab.User)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
        }
    }
}
