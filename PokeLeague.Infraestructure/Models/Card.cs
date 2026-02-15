using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class Card
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string SetId { get; set; } = null!;

    public string RarityId { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? Grade { get; set; }

    public bool IsNew { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Auction> Auction { get; set; } = new List<Auction>();

    public virtual ICollection<CategoryCard> CategoryCard { get; set; } = new List<CategoryCard>();

    public virtual ICollection<Image> Image { get; set; } = new List<Image>();

    public virtual Language LanguageCodeNavigation { get; set; } = null!;

    public virtual Rarity Rarity { get; set; } = null!;

    public virtual Set Set { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
