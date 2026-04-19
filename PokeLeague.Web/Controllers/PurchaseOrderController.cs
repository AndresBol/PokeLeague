using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Web.Util;
using System.Security.Claims;

namespace PokeLeague.Web.Controllers
{
    [Authorize]
    public class PurchaseOrderController : Controller
    {
        private readonly IServicePurchaseOrder _servicePurchaseOrder;
        private readonly IServiceAuction _serviceAuction;

        public PurchaseOrderController(IServicePurchaseOrder servicePurchaseOrder, IServiceAuction serviceAuction)
        {
            _servicePurchaseOrder = servicePurchaseOrder;
            _serviceAuction = serviceAuction;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<PurchaseOrderDTO> payments;

            if (User.IsInRole("Admin"))
            {
                payments = await _servicePurchaseOrder.ListAsync();
            }
            else
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                payments = await _servicePurchaseOrder.ListByUserIdAsync(currentUserId);
            }

            return View(payments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Payment not found",
                    "No payment ID was provided.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }

            var payment = await _servicePurchaseOrder.FindByIdAsync(id.Value);
            if (payment == null)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Payment not found",
                    $"No payment found with ID {id.Value}.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (payment.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Unauthorized",
                    "You can only view your own payments.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPayment(int auctionId)
        {
            try
            {
                var auction = await _serviceAuction.FindByIdAsync(auctionId);
                if (auction == null || auction.Status != "Finished" || !auction.AuctionBid.Any())
                {
                    TempData["Notification"] = SweetAlertHelper.CreateNotification(
                        "Error",
                        "This auction is not eligible for payment registration.",
                        SweetAlertMessageType.error
                    );
                    return RedirectToAction("Details", "Auction", new { id = auctionId });
                }

                var winner = auction.AuctionBid.OrderByDescending(b => b.BidAmount).First();
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int currentUserId = int.Parse(userIdClaim!);

                if (currentUserId != winner.UserId)
                {
                    TempData["Notification"] = SweetAlertHelper.CreateNotification(
                        "Unauthorized",
                        "Only the auction winner can register the payment.",
                        SweetAlertMessageType.error
                    );
                    return RedirectToAction("Details", "Auction", new { id = auctionId });
                }

                var newId = await _servicePurchaseOrder.RegisterPaymentForAuctionAsync(auctionId);

                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Payment registered",
                    "The payment was registered successfully.",
                    SweetAlertMessageType.success
                );
                return RedirectToAction(nameof(Details), new { id = newId });
            }
            catch (Exception ex)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Error",
                    ex.Message,
                    SweetAlertMessageType.error
                );
                return RedirectToAction("Details", "Auction", new { id = auctionId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            try
            {
                var payment = await _servicePurchaseOrder.FindByIdAsync(id);
                if (payment == null)
                {
                    TempData["Notification"] = SweetAlertHelper.CreateNotification(
                        "Error",
                        "Payment not found.",
                        SweetAlertMessageType.error
                    );
                    return RedirectToAction(nameof(Index));
                }

                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int currentUserId = int.Parse(userIdClaim!);

                if (currentUserId != payment.UserId)
                {
                    TempData["Notification"] = SweetAlertHelper.CreateNotification(
                        "Unauthorized",
                        "Only the buyer can confirm the payment.",
                        SweetAlertMessageType.error
                    );
                    return RedirectToAction(nameof(Details), new { id });
                }

                await _servicePurchaseOrder.ConfirmPaymentAsync(id);

                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Payment confirmed",
                    "The payment has been confirmed successfully.",
                    SweetAlertMessageType.success
                );
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Error",
                    ex.Message,
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Details), new { id });
            }
        }
    }
}
