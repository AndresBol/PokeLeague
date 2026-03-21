using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Web.Util;

namespace PokeLeague.Web.Controllers
{
    public class CardController : Controller
    {
        private readonly IServiceCard _serviceCard;
        private readonly IServiceCategory _serviceCategory;
        private readonly IServiceUser _serviceUser;


        public CardController(IServiceCard serviceCard, IServiceCategory serviceCategory, IServiceUser serviceUser)
        {
            _serviceCard = serviceCard;
            _serviceCategory = serviceCategory;
            _serviceUser = serviceUser;
        }

        public async Task<IActionResult> Index()
        {
            var cards = await _serviceCard.ListAsync();
            return View(cards);
        }

        public async Task <IActionResult> Create()
        {
            await LoadCombosAsync();
            return View();
        }
        private async Task LoadCombosAsync() 
        {
            ViewBag.Categories = await _serviceCategory.ListAsync();

            var users = await _serviceUser.ListAsync();
            ViewBag.User = users.FirstOrDefault();
        }

        public async Task<IActionResult> Edit(int ? id) 
        {
            if (id == null)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                     "Card not found",
                     "No card ID was provided.",
                     SweetAlertMessageType.error
                    );
                return RedirectToAction(nameof(Index));
            }

            var card = await _serviceCard.FindByIdAsync(id.Value);

            if (card == null) 
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Card no found",
                    $"No card found with the ID {id.Value}.",
                    SweetAlertMessageType.error
                    );
                return RedirectToAction(nameof(Index));
            }

            if(card.AuctionStatus != "Scheduled") 
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                       "Edit not allowed",
                       "This card cannot be edited because it is in an active auction.",
                       SweetAlertMessageType.warning
                    );

                return RedirectToAction(nameof(Details), new { id });
            }

            await LoadCombosAsync();

            return View(card);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CardDTO cardDTO)
        {
            if (string.IsNullOrWhiteSpace(cardDTO.Name)) 
            {
                ModelState.AddModelError("Name", "Name is required");
            }

            if(cardDTO.Description ==null || cardDTO.Description.Length < 20) 
            {
                ModelState.AddModelError("Description", "Description must be at least 20 characters.");
            }

            if(cardDTO.CategoryCard == null || !cardDTO.CategoryCard.Any()) 
            {
                ModelState.AddModelError("", "At least one category is required.");
            }

            if(cardDTO.Image == null || !cardDTO.Image.Any())
            {
                ModelState.AddModelError("", "At least one image is required.");
            }

            if (!ModelState.IsValid) 
            {
                ViewBag.Notification = SweetAlertHelper.CreateNotification(
                        "Validation errors",
                        "Please complete all required fields",
                        SweetAlertMessageType.warning
                    );
                await LoadCombosAsync();
                return View(cardDTO);
            }

            cardDTO.IsActive = true;

            var users = await _serviceUser.ListAsync();
            var currentUser = users.FirstOrDefault();
            cardDTO.UserId = currentUser.Id;

            await _serviceCard.AddAsync(cardDTO);

            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Card created",
                "The card was created successfully.",
                SweetAlertMessageType.success

                );

            return RedirectToAction(nameof(Index)); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, CardDTO cardDTO)
        {
            if(id != cardDTO.Id)
            {
                return NotFound();
            }

            var existing = await _serviceCard.FindByIdAsync(id);

            if (existing == null)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                        "Card not found",
                        $"No card found with ID {id}.",
                        SweetAlertMessageType.error

           );

                return RedirectToAction(nameof(Index));
            }

            if(existing.AuctionStatus != "Scheduled") 
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                        "Edit not allowed",
                        "This card cannot be edited because it is in an active auction.",
                        SweetAlertMessageType.warning
                        );
                return RedirectToAction(nameof(Details), new { id });
            }

            if (string.IsNullOrWhiteSpace(cardDTO.Name))
            {
                ModelState.AddModelError("Name", "Namse is required.");
            }

            if (cardDTO.Description == null || cardDTO.Description.Length < 20) 
            {
                ModelState.AddModelError("Description", "Description must be at least 20 charaters.");
            }

            if (cardDTO.CategoryCard == null || !cardDTO.CategoryCard.Any()) 
            {
                ModelState.AddModelError("", "At least one category is required.");
            }

            if(cardDTO.Image == null || !cardDTO.Image.Any()) 
            {
                ModelState.AddModelError("", "At least one image is required.");
            }

            if (!ModelState.IsValid) 
            {
                ViewBag.Notification = SweetAlertHelper.CreateNotification(
                    "Validation errors",
                    "Please correct the errors in the form.",
                    SweetAlertMessageType.warning
                    );

                await LoadCombosAsync();
                return View(cardDTO);
            
            }

            cardDTO.UserId = existing.UserId;
            cardDTO.IsActive = existing.IsActive;
            cardDTO.RegistrationDate = existing.RegistrationDate;

            await _serviceCard.UpdateAsync(cardDTO);

            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                   "Card update",
                   "The card was updated successfully.",
                   SweetAlertMessageType.success
                );

            return RedirectToAction(nameof(Details), new { id = cardDTO.Id });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) 
        {
            var card = await _serviceCard.FindByIdAsync(id);

            if(card == null) 
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Card not found",
                    $"No card found with the ID {id}.",
                    SweetAlertMessageType.error

                    );
                return RedirectToAction(nameof(Index));
            }

            if (card.AuctionStatus == "In Progress")
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Delete not allowed",
                    "This card is in an active auction.",
                    SweetAlertMessageType.warning
                    
                    );
                return RedirectToAction(nameof(Details), new { id });
            }

            if (card.Auction != null && card.Auction.Any()) 
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                 "Delete not allowed",
                 "This card has already been acutioned.",
                 SweetAlertMessageType.warning

                 );
                return RedirectToAction(nameof(Details), new { id });
            }

            await _serviceCard.DeleteAsync(id);

            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Card deleted",
                "This card was removed successfully.",
                SweetAlertMessageType.warning
                );

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id) 
        {
            var card = await _serviceCard.FindByIdAsync(id);

            if(card == null) 
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Card not found",
                    $"No card found with ID {id}.",
                    SweetAlertMessageType.error
                    );
                return RedirectToAction(nameof(Index));
            }

            if(card.AuctionStatus == "In Progress") 
            {
                TempData["Notication"] = SweetAlertHelper.CreateNotification(
                    "Action not allowed",
                    "This card cannot be modified because it is in an active auction.",
                    SweetAlertMessageType.warning
                    );
                return RedirectToAction(nameof(Details), new { id });
            }

            await _serviceCard.ToggleActiveAsync(id);

            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Status updated",
                "The card status was updated successfully.",
                SweetAlertMessageType.success

                );

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
