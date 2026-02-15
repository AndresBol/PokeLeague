using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class AuctionBid
{
    public int Id { get; set; }

    public int AuctionId { get; set; }

    public int UserId { get; set; }

    public decimal BidAmount { get; set; }

    public DateTime BidDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Auction Auction { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
