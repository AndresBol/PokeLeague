using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
        public DateOnly SignupDate { get; set; }
        public bool IsActive { get; set; }
        public RoleDTO Role { get; set; } = new RoleDTO();
    }
}