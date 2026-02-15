using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class Auction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CardId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal BasePrice { get; set; }

    public decimal MinIncrease { get; set; }

    public bool IsCanceled { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AuctionBid> AuctionBid { get; set; } = new List<AuctionBid>();

    public virtual Card Card { get; set; } = null!;

    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    public virtual User User { get; set; } = null!;
}
