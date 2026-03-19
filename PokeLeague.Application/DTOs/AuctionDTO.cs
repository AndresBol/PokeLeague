using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.DTOs
{
    public record AuctionDTO
    {
        [DisplayName("Auction Identificator")]
        public int Id { get; set; }
        [DisplayName("User")]
        [Required]
        public int UserId { get; set; }
        [DisplayName("Card")]
        [Required]
        public int CardId { get; set; }
        [DisplayName("Start Date")]
        [Required]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        [Required]
        public DateTime EndDate { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Base price must be greater than 0.")]
        public decimal BasePrice { get; set; }
        [DisplayName("Min Increase")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Minimum bid increment must be greater than 0.")]
        public decimal MinIncrease { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsActive { get; set; }
        public CardDTO Card { get; set; } = new CardDTO();
        public PurchaseOrderDTO? PurchaseOrder { get; set; }
        public ICollection<AuctionBidDTO> AuctionBid { get; set; } = new List<AuctionBidDTO>();
        public string Status { get; set; } = string.Empty;
    }
}
