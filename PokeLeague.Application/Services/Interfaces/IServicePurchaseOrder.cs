using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServicePurchaseOrder
    {
        Task<ICollection<PurchaseOrderDTO>> ListAsync();
        Task<ICollection<PurchaseOrderDTO>> ListByUserIdAsync(int userId);
        Task<PurchaseOrderDTO> FindByIdAsync(int id);
        Task<PurchaseOrderDTO?> FindByAuctionIdAsync(int auctionId);
        Task<int> AddAsync(PurchaseOrderDTO purchaseOrderDto);
        Task UpdateAsync(PurchaseOrderDTO purchaseOrderDto);
        Task DeleteAsync(int id);
        Task<int> RegisterPaymentForAuctionAsync(int auctionId);
        Task ConfirmPaymentAsync(int id);
    }
}
