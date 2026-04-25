using System.ComponentModel.DataAnnotations;

namespace PokeLeague.Web.ViewModels
{
    public class ViewModelReport
    {
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Auction Status")]
        public string? Status { get; set; }

        public string TituloGrafico { get; set; } = "Bids per Auction";

        public List<string> Etiquetas { get; set; } = new();

        public List<int> Valores { get; set; } = new();

        public string? Mensaje { get; set; }

        public bool TieneDatos => Valores != null && Valores.Count > 0;
    }
}
