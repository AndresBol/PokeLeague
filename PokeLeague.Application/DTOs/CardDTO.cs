using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record CardDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SetId { get; set; } = string.Empty;
        public string RarityId { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;

        [DisplayName("Card Name")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal? Grade { get; set; }
        [DisplayName("Status")]
        public bool IsNew { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        public SetDTO Set { get; set; } = new SetDTO();
        public RarityDTO Rarity { get; set; } = new RarityDTO();
        public LanguageDTO Language { get; set; } = new LanguageDTO();
        public ICollection<ImageDTO> Image { get; set; } = new List<ImageDTO>();
        public ICollection<CategoryCardDTO> CategoryCard { get; set; } = new List<CategoryCardDTO>();
        public UserDTO User { get; set; } = new UserDTO();
    }
}
