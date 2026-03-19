using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;

namespace PokeLeague.Web.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IServiceAuction _serviceAuction;
        private readonly IServiceUser _serviceUser;
        private readonly IServiceRole _serviceRole;

        public AuctionController(IServiceAuction serviceAuction, IServiceUser serviceUser, IServiceRole serviceRole)
        {
            _serviceAuction = serviceAuction;
            _serviceUser = serviceUser;
            _serviceRole = serviceRole;
        }

        public async Task<IActionResult> Index()
        {
            var autions = await _serviceAuction.ListAsync();

            return View(autions);
        }

        public async Task<IActionResult> Create()
        {
            await LoadCombosAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuctionDTO auctionDto)
        {
            if (auctionDto.EndDate <= auctionDto.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date must be after start date.");
            }

            var activeAuction = await _serviceAuction.FindActiveByCardIdAsync(auctionDto.CardId);
            if (activeAuction != null)
            {
                ModelState.AddModelError("CardId", "This card already has an active auction.");
            }

            if (!ModelState.IsValid)
            {
                await LoadCombosAsync();
                return View(auctionDto);
            }

            auctionDto.IsActive = true;
            await _serviceAuction.AddAsync(auctionDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _serviceAuction.FindByIdAsync(id.Value);
            return View(auction);
        }

        public async Task<IActionResult> Bids(int id)
        {
            Console.WriteLine($"ID received: {id}");
            var auction = await _serviceAuction.FindByIdAsync(id);
            if (auction == null)
                return NotFound();

            return View(auction);
        }

        public async Task<IActionResult> Active()
        {
            var auctions = await _serviceAuction.ListActiveAsync();
            return View("Index", auctions);
        }

        public async Task<IActionResult> Closed()
        {
            var auctions = await _serviceAuction.ListClosedAsync();
            return View("Index", auctions);
        }

        private async Task LoadCombosAsync(IEnumerable<string>? selectedCategoriaIds = null)
        {
            var roleDto = await _serviceRole.FindByNameAsync("Seller");
            ViewBag.ListUser = await _serviceUser.ListByRoleAsync(roleDto);
        }

        [HttpGet]
        public async Task<JsonResult> GetCardsByUser(int userId)
        {
            var user = await _serviceUser.FindByIdAsync(userId);
            var cards = user?.Card?.Select(c => new { c.Id, c.Name }) ?? [];
            return Json(cards);
        }

        [HttpGet]
        public async Task<JsonResult> HasActiveAuction(int cardId)
        {
            var activeAuction = await _serviceAuction.FindActiveByCardIdAsync(cardId);
            return Json(new { hasActive = activeAuction != null });
        }
    }
}
