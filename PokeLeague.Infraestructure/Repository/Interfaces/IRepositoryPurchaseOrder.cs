using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPurchaseOrder
    {
        Task<ICollection<PurchaseOrder>> ListAsync();
        Task<ICollection<PurchaseOrder>> ListByUserIdAsync(int userId);
        Task<PurchaseOrder> FindByIdAsync(int id);
        Task<PurchaseOrder?> FindByAuctionIdAsync(int auctionId);
        Task<int> AddAsync(PurchaseOrder purchaseOrder);
        Task UpdateAsync(PurchaseOrder purchaseOrder);
        Task DeleteAsync(int id);
    }
}
