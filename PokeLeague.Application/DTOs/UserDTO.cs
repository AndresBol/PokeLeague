using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record UserDTO
    {
        public int Id { get; set; }
        [DisplayName("Full Name")]
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        [DisplayName("Account Status")]
        public bool IsBlocked { get; set; }
        [DisplayName("Signup Date")]
        public DateOnly SignupDate { get; set; }
        public ICollection<AuctionDTO> Auction { get; set; } = new List<AuctionDTO>();
        public ICollection<AuctionBidDTO> AuctionBid { get; set; } = new List<AuctionBidDTO>();
        public bool IsActive { get; set; }
        public RoleDTO Role { get; set; } = new RoleDTO();
        public ICollection<CardDTO> Card { get; set; } = new List<CardDTO>();
    }
}