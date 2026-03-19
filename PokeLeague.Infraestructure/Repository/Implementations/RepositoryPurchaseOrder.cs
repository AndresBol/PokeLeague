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
    public class RepositoryPurchaseOrder : IRepositoryPurchaseOrder
    {
        private readonly PokeLeagueContext _context;

        public RepositoryPurchaseOrder(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(PurchaseOrder purchaseOrder)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Set<PurchaseOrder>().AddAsync(purchaseOrder);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return purchaseOrder.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error adding purchase order: {ex.Message}");
                }
            });
        }

        public async Task<PurchaseOrder> FindByIdAsync(int id)
        {
            var purchaseOrder = await _context.Set<PurchaseOrder>()
                .AsNoTracking()
                .FirstOrDefaultAsync(po => po.Id == id && po.IsActive);
            return purchaseOrder!;
        }

        public async Task<ICollection<PurchaseOrder>> ListAsync()
        {
            var purchaseOrders = await _context.Set<PurchaseOrder>()
                .AsNoTracking()
                .Include(po => po.Auction)
                    .ThenInclude(a => a.Card)
                .Include(po => po.Auction)
                    .ThenInclude(a => a.User)
                .Include(po => po.User)
                    .ThenInclude(u => u.Role)
                .Where(po => po.IsActive)
                .OrderBy(po => po.Id)
                .ToListAsync();
            return purchaseOrders!;
        }

        public async Task UpdateAsync(PurchaseOrder purchaseOrder)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existingPurchaseOrder = await _context.Set<PurchaseOrder>()
                        .FirstOrDefaultAsync(po => po.Id == purchaseOrder.Id);

                    if (existingPurchaseOrder == null)
                    {
                        throw new Exception($"PurchaseOrder with ID {purchaseOrder.Id} not found.");
                    }

                    existingPurchaseOrder.AuctionId = purchaseOrder.AuctionId;
                    existingPurchaseOrder.UserId = purchaseOrder.UserId;
                    existingPurchaseOrder.PurchaseAmount = purchaseOrder.PurchaseAmount;
                    existingPurchaseOrder.IsPaid = purchaseOrder.IsPaid;
                    existingPurchaseOrder.IsActive = purchaseOrder.IsActive;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error updating purchase order: {ex.Message}");
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
                    var purchaseOrder = await _context.Set<PurchaseOrder>().FirstOrDefaultAsync(po => po.Id == id);

                    if (purchaseOrder == null)
                    {
                        throw new Exception($"PurchaseOrder with ID {id} not found.");
                    }

                    purchaseOrder.IsActive = false;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error deleting purchase order: {ex.Message}");
                }
            });
        }
    }
}
