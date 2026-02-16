using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
