using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record AuctionBidViewDTO
    {
        public int Id { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime BidDate { get; set; }
        public string Username { get; set; } = string.Empty;

    }
}