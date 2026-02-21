using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record CategoryCardDTO
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public CategoryDTO Category { get; set; } = new CategoryDTO();
    }
}
