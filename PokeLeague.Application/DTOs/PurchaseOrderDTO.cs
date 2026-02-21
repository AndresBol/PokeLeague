using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record PurchaseOrderDTO
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        public decimal PurchaseAmount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsActive { get; set; }
    }
}
