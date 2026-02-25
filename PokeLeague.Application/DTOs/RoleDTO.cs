using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record RoleDTO
    {
        public int Id { get; set; }
        [DisplayName("Role")]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
