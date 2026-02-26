using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record AuctionBidDTO
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        [DisplayName("Amount")]
        public decimal BidAmount { get; set; }
        [DisplayName("Date")]
        public DateTime BidDate { get; set; }
        public bool IsActive { get; set; }
        public UserDTO User { get; set; } = new UserDTO();
    }
}
