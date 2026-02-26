using Microsoft.AspNetCore.Mvc;
using PokeLeague.Application.Services.Interfaces;

namespace PokeLeague.Web.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IServiceAuction _serviceAuction;

        public AuctionController(IServiceAuction serviceAuction)
        {
            _serviceAuction = serviceAuction;
        }

        public async Task <IActionResult> Index()
        {
            var autions = await _serviceAuction.ListAsync();

            return View(autions);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var auction = await _serviceAuction.FindByIdAsync(id.Value);
            return View(auction);
        }
    }
}
