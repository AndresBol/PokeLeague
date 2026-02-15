using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class PurchaseOrder
{
    public int Id { get; set; }

    public int AuctionId { get; set; }

    public int UserId { get; set; }

    public decimal PurchaseAmount { get; set; }

    public bool IsPaid { get; set; }

    public bool IsActive { get; set; }

    public virtual Auction Auction { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
