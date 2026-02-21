using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record ImageDTO
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public byte[] ImageData { get; set; } = [];
        public bool IsActive { get; set; }
    }
}
