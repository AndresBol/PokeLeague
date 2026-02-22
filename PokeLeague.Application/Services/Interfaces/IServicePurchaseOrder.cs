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
        Task<PurchaseOrderDTO> FindByIdAsync(int id);
        Task<int> AddAsync(PurchaseOrderDTO purchaseOrderDto);
        Task UpdateAsync(PurchaseOrderDTO purchaseOrderDto);
        Task DeleteAsync(int id);
    }
}
