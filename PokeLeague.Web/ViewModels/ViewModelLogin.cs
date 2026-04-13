using System.ComponentModel.DataAnnotations;

namespace PokeLeague.Web.ViewModels
{
    public record ViewModelLogin
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;

        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 15 characters")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only letters and numbers allowed")]
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Password")]
        public string Password { get; set; } = default!;
    }
}
