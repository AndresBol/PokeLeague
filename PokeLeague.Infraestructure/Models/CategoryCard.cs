using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class CategoryCard
{
    public int Id { get; set; }

    public int CardId { get; set; }

    public int CategoryId { get; set; }

    public bool IsActive { get; set; }

    public virtual Card Card { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}
