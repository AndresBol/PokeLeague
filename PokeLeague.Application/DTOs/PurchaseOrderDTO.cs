using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record PurchaseOrderDTO
    {
        public int Id { get; set; }
        [DisplayName("Auction")]
        public int AuctionId { get; set; }
        [DisplayName("Buyer")]
        public int UserId { get; set; }
        [DisplayName("Amount")]
        public decimal PurchaseAmount { get; set; }
        [DisplayName("Payment Date")]
        public DateTime PaymentDate { get; set; }
        public bool IsPaid { get; set; }
        public bool IsActive { get; set; }
        public AuctionDTO Auction { get; set; } = new AuctionDTO();
        public UserDTO User { get; set; } = new UserDTO();
        [DisplayName("Payment Status")]
        public string Status { get; set; } = string.Empty;
    }
}
