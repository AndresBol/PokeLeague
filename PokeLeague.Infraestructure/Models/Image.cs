using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class Image
{
    public int Id { get; set; }

    public int CardId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Card Card { get; set; } = null!;
}
