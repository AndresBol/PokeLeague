using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class User
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsBlocked { get; set; }

    public DateOnly SignupDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Auction> Auction { get; set; } = new List<Auction>();

    public virtual ICollection<AuctionBid> AuctionBid { get; set; } = new List<AuctionBid>();

    public virtual ICollection<Card> Card { get; set; } = new List<Card>();

    public virtual ICollection<PurchaseOrder> PurchaseOrder { get; set; } = new List<PurchaseOrder>();

    public virtual Role Role { get; set; } = null!;
}
