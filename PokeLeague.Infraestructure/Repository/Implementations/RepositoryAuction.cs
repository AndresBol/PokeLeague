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
            try
            {
                await _context.Database.BeginTransactionAsync();
                await _context.Set<Auction>().AddAsync(auction);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return auction.Id;
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error adding auction: {ex.Message}");
            }
        }

        public async Task<Auction> FindByIdAsync(int id)
        {
            var auction = await _context.Set<Auction>()
                .AsNoTracking()
                .Include(a => a.Card)
                .Include(a => a.AuctionBid)
                .Include(a => a.PurchaseOrder)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
            return auction!;
        }

        public async Task<ICollection<Auction>> ListAsync()
        {
            var auctions = await _context.Set<Auction>()
                .AsNoTracking()
                .Where(a => a.IsActive)
                .OrderBy(a => a.Id)
                .ToListAsync();
            return auctions!;
        }

        public async Task UpdateAsync(Auction auction)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

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

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error updating auction: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var auction = await _context.Set<Auction>().FirstOrDefaultAsync(a => a.Id == id);

                if (auction == null)
                {
                    throw new Exception($"Auction with ID {id} not found.");
                }

                auction.IsActive = false;
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error deleting auction: {ex.Message}");
            }
        }
    }
}
