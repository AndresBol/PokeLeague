using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record LanguageDTO
    {
        public string LanguageCode { get; set; } = string.Empty;
        [DisplayName("Language")]
        public string LanguageName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
