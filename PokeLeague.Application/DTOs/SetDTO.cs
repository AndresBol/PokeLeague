using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record SetDTO
    {
        public string Id { get; set; } = string.Empty;
        [DisplayName("Set")]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
