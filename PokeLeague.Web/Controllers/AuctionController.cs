using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Web.Util;
using System.Security.Claims;

namespace PokeLeague.Web.Controllers
{
    [Authorize]
    public class AuctionController : Controller
    {
        private readonly IServiceAuction _serviceAuction;
        private readonly IServiceUser _serviceUser;

        public AuctionController(IServiceAuction serviceAuction, IServiceUser serviceUser)
        {
            _serviceAuction = serviceAuction;
            _serviceUser = serviceUser;
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Auction not found",
                    "No auction ID was provided.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }

            var auction = await _serviceAuction.FindByIdAsync(id.Value);
            if (auction == null)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Auction not found",
                    $"No auction found with ID {id.Value}.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }

            if (auction.Status != "Scheduled" || auction.AuctionBid.Count > 0)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Edit not allowed",
                    "This auction cannot be edited because it has already started or has bids.",
                    SweetAlertMessageType.warning
                );
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(auction);
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
                ViewBag.Notification = SweetAlertHelper.CreateNotification(
                    "Validation errors",
                    "Please correct the errors in the form.",
                    SweetAlertMessageType.warning
                );
                await LoadCombosAsync();
                return View(auctionDto);
            }

            auctionDto.IsActive = true;

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            auctionDto.UserId = int.Parse(userIdClaim!);

            await _serviceAuction.AddAsync(auctionDto);
            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Auction created",
                "The auction was published successfully.",
                SweetAlertMessageType.success
            );
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Auction not found",
                    "No auction ID was provided.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }

            var auction = await _serviceAuction.FindByIdAsync(id.Value);
            if (auction == null)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Auction not found",
                    $"No auction found with ID {id.Value}.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }
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
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userIdClaim!);
            var user = await _serviceUser.FindByIdAsync(userId);
            ViewBag.LoggedUser = user;
            ViewBag.LoggedUserCards = user?.Card?.Select(c => new { c.Id, c.Name }) ?? [];
        }

        [HttpGet]
        public async Task<JsonResult> GetCardsByUser()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userIdClaim!);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuctionDTO auctionDto)
        {
            if (id != auctionDto.Id)
            {
                return NotFound();
            }

            var existing = await _serviceAuction.FindByIdAsync(id);
            if (existing == null)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Auction not found",
                    $"No auction found with ID {id}.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }

            if (existing.Status != "Scheduled" || existing.AuctionBid.Count > 0)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Edit not allowed",
                    "This auction cannot be edited because it has already started or has bids.",
                    SweetAlertMessageType.warning
                );
                return RedirectToAction(nameof(Details), new { id });
            }

            if (auctionDto.EndDate <= auctionDto.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date must be after start date.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Notification = SweetAlertHelper.CreateNotification(
                    "Validation errors",
                    "Please correct the errors in the form.",
                    SweetAlertMessageType.warning
                );
                return View(auctionDto);
            }

            auctionDto.UserId = existing.UserId;
            auctionDto.CardId = existing.CardId;
            auctionDto.IsActive = existing.IsActive;
            auctionDto.IsCanceled = existing.IsCanceled;

            await _serviceAuction.UpdateAsync(auctionDto);
            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Auction updated",
                "The auction was updated successfully.",
                SweetAlertMessageType.success
            );
            return RedirectToAction(nameof(Details), new { id = auctionDto.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var auction = await _serviceAuction.FindByIdAsync(id);
            if (auction == null)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Auction not found",
                    $"No auction found with ID {id}.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }

            if (auction.Status != "Scheduled" || auction.AuctionBid.Count > 0)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Cancel not allowed",
                    "This auction cannot be canceled because it has already started or has bids.",
                    SweetAlertMessageType.warning
                );
                return RedirectToAction(nameof(Details), new { id });
            }

            auction.IsCanceled = true;
            await _serviceAuction.UpdateAsync(auction);
            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Auction canceled",
                "The auction was canceled successfully.",
                SweetAlertMessageType.success
            );
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
