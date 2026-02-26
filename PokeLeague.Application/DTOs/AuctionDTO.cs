using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record AuctionDTO
    {
        [DisplayName("Auction Identificator")]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CardId { get; set; }
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        public decimal BasePrice { get; set; }
        public decimal MinIncrease { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsActive { get; set; }
        public CardDTO Card { get; set; } = new CardDTO();
        public PurchaseOrderDTO? PurchaseOrder { get; set; }
        public ICollection<AuctionBidDTO> AuctionBid { get; set; } = new List<AuctionBidDTO>();
        public string Status { get; set; } = string.Empty;
    }
}
