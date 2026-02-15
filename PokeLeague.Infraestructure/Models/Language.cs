using System;
using System.Collections.Generic;

namespace PokeLeague.Infraestructure.Models;

public partial class Language
{
    public string LanguageCode { get; set; } = null!;

    public string LanguageName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Card> Card { get; set; } = new List<Card>();
}
