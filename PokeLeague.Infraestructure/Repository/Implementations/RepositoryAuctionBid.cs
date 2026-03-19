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
    public class RepositoryAuctionBid : IRepositoryAuctionBid
    {
        private readonly PokeLeagueContext _context;

        public RepositoryAuctionBid(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(AuctionBid auctionBid)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Set<AuctionBid>().AddAsync(auctionBid);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return auctionBid.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error adding auction bid: {ex.Message}");
                }
            });
        }

        public async Task<AuctionBid> FindByIdAsync(int id)
        {
            var auctionBid = await _context.Set<AuctionBid>()
                .AsNoTracking()
                .Include(ab => ab.Auction)
                    .ThenInclude(a => a.Card)
                .Include(ab => ab.Auction)
                    .ThenInclude(a => a.User)
                .Include(ab => ab.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(ab => ab.Id == id && ab.IsActive);
            return auctionBid!;
        }

        public async Task<ICollection<AuctionBid>> ListAsync()
        {
            var auctionBids = await _context.Set<AuctionBid>()
                .AsNoTracking()
                .Include(ab => ab.Auction)
                    .ThenInclude(a => a.Card)
                .Include(ab => ab.Auction)
                    .ThenInclude(a => a.User)
                .Include(ab => ab.User)
                    .ThenInclude(u => u.Role)
                .Where(ab => ab.IsActive)
                .OrderByDescending(ab => ab.BidDate)
                .ToListAsync();
            return auctionBids!;
        }

        public async Task UpdateAsync(AuctionBid auctionBid)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existingAuctionBid = await _context.Set<AuctionBid>()
                        .FirstOrDefaultAsync(ab => ab.Id == auctionBid.Id);

                    if (existingAuctionBid == null)
                    {
                        throw new Exception($"AuctionBid with ID {auctionBid.Id} not found.");
                    }

                    existingAuctionBid.AuctionId = auctionBid.AuctionId;
                    existingAuctionBid.UserId = auctionBid.UserId;
                    existingAuctionBid.BidAmount = auctionBid.BidAmount;
                    existingAuctionBid.BidDate = auctionBid.BidDate;
                    existingAuctionBid.IsActive = auctionBid.IsActive;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error updating auction bid: {ex.Message}");
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
                    var auctionBid = await _context.Set<AuctionBid>().FirstOrDefaultAsync(ab => ab.Id == id);

                    if (auctionBid == null)
                    {
                        throw new Exception($"AuctionBid with ID {id} not found.");
                    }

                    auctionBid.IsActive = false;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error deleting auction bid: {ex.Message}");
                }
            });
        }
    }
}
