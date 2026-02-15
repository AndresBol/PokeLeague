using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<CategoryCard> CategoryCard { get; set; } = new List<CategoryCard>();
}
