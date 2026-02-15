using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<User> User { get; set; } = new List<User>();
}
