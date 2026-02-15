using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class Set
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Card> Card { get; set; } = new List<Card>();
}
