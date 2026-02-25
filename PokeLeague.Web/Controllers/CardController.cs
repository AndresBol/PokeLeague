using Microsoft.AspNetCore.Mvc;
using PokeLeague.Application.Services.Interfaces;

namespace PokeLeague.Web.Controllers
{
    public class CardController : Controller
    {
        private readonly IServiceCard _serviceCard;

        public CardController(IServiceCard serviceCard)
        {
            _serviceCard = serviceCard;
        }

        public async Task<IActionResult> Index()
        {
            var cards = await _serviceCard.ListAsync();
            return View(cards);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var card = await _serviceCard.FindByIdAsync(id.Value);

            if(card == null) {
                return NotFound();
            }

            return View(card);
        }
    }
}
