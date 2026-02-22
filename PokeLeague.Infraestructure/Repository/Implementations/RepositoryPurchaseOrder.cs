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
            try
            {
                await _context.Database.BeginTransactionAsync();
                await _context.Set<PurchaseOrder>().AddAsync(purchaseOrder);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return purchaseOrder.Id;
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error adding purchase order: {ex.Message}");
            }
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
                .Where(po => po.IsActive)
                .OrderBy(po => po.Id)
                .ToListAsync();
            return purchaseOrders!;
        }

        public async Task UpdateAsync(PurchaseOrder purchaseOrder)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

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

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error updating purchase order: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var purchaseOrder = await _context.Set<PurchaseOrder>().FirstOrDefaultAsync(po => po.Id == id);

                if (purchaseOrder == null)
                {
                    throw new Exception($"PurchaseOrder with ID {id} not found.");
                }

                purchaseOrder.IsActive = false;
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error deleting purchase order: {ex.Message}");
            }
        }
    }
}
