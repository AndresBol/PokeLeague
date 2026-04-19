using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Web.Util;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace PokeLeague.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CardController : Controller
    {
        private readonly IServiceCard _serviceCard;
        private readonly IServiceCategory _serviceCategory;
        private readonly IServiceLanguage _serviceLanguage;
        private readonly IServiceRarity _serviceRarity;
        private readonly IServiceSet _serviceSet;

        public CardController(IServiceCard serviceCard, IServiceCategory serviceCategory, IServiceLanguage serviceLanguage, IServiceRarity serviceRarity, IServiceSet serviceSet)
        {
            _serviceCard = serviceCard;
            _serviceCategory = serviceCategory;
            _serviceLanguage = serviceLanguage;
            _serviceRarity = serviceRarity;
            _serviceSet = serviceSet;
        }

        public async Task<IActionResult> Index()
        {
            var cards = await _serviceCard.ListAsync();
            return View(cards);
        }

        public async Task<IActionResult> Create()
        {
            await LoadCombosAsync();
            return View();
        }
        public async Task<IActionResult> GuidedCreate()
        {
            await LoadCombosAsync();
            return View();
        }
        private async Task LoadCombosAsync() 
        {
            var cats = await _serviceCategory.ListAsync();

            ViewBag.Categories = cats;

            ViewBag.Username = User.Identity?.Name;

            ViewBag.Languages = await _serviceLanguage.ListAsync();

            ViewBag.Rarity = await _serviceRarity.ListAsync();

            ViewBag.Set= await _serviceSet.ListAsync();
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

            CardDTO card;
            try
            {
                card = await _serviceCard.FindByIdAsync(id.Value);
            }
            catch (Exception)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Card not found",
                    $"No card found with the ID {id.Value}.",
                    SweetAlertMessageType.error
                    );
                return RedirectToAction(nameof(Index));
            }

            if(card.AuctionStatus == "In Progress") 
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                       "Edit not allowed",
                       "This card cannot be edited because it is in an active auction.",
                       SweetAlertMessageType.warning
                    );

                return RedirectToAction(nameof(Details), new { id });
            }

            await LoadCombosAsync();

            ViewBag.SelectedCategories = card.CategoryCard?
                .Where(c => c.IsActive)
                .Select(c => c.CategoryId)
                .ToList();
              

            return View(card);


        }

        public void FormValidation(CardDTO cardDTO, List<IFormFile> files, List<int> selectedCategoryIds)
        {
            if (string.IsNullOrWhiteSpace(cardDTO.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
            }

            if (cardDTO.Description == null || cardDTO.Description.Length < 20)
            {
                ModelState.AddModelError("Description", "Description must be at least 20 characters.");
            }

            if (selectedCategoryIds == null || selectedCategoryIds.Count == 0)
            {
                ModelState.AddModelError("selectedCategoryIds", "At least one category is required.");
            }

            if (files == null || files.Count == 0)
            {
                ModelState.AddModelError("files", "At least one image is required.");
            }

            if (string.IsNullOrEmpty(cardDTO.SetId)) 
            {
                ModelState.AddModelError("SetId", "Please select a set.");
            }

            if (string.IsNullOrEmpty(cardDTO.RarityId)) 
            {
                ModelState.AddModelError("RarityId", "Please select a rarity.");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CardDTO cardDTO, List<IFormFile> files, List<int> selectedCategoryIds)
        {
            FormValidation(cardDTO, files, selectedCategoryIds);
            if (!ModelState.IsValid)
            {
                ViewBag.Notification = SweetAlertHelper.CreateNotification(
                        "Validation errors",
                        "Please complete all required fields",
                        SweetAlertMessageType.warning
                    );

                ViewBag.SelectedCategories = selectedCategoryIds;

                await LoadCombosAsync();
                return View(cardDTO);
            }


            cardDTO.IsActive = true;
            cardDTO.RegistrationDate = DateOnly.FromDateTime(DateTime.Now);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cardDTO.UserId = int.Parse(userIdClaim!);


            List<CategoryCardDTO> categories = [];
            if (selectedCategoryIds != null && selectedCategoryIds.Count > 0)
            {
                foreach (var catId in selectedCategoryIds)
                {
                    categories.Add(new CategoryCardDTO
                    {
                        CategoryId = catId,
                        IsActive = true
                    });
                }
            }
            cardDTO.CategoryCard = categories;


            List<ImageDTO> images = [];
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);

                    var imageBytes = ms.ToArray();

                    images.Add(new ImageDTO
                    {
                        ImageData = imageBytes,
                        IsActive = true
                    });
                }

            }
            cardDTO.Image = images;

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
        public async Task<IActionResult> Edit (CardDTO cardDTO,List<IFormFile> files, List<int> selectedCategoryIds)
        {
            FormValidation(cardDTO, files, selectedCategoryIds);
            if (!ModelState.IsValid)
            {
                ViewBag.Notification = SweetAlertHelper.CreateNotification(
                    "Validation errors",
                    "Please correct the errors in the form.",
                    SweetAlertMessageType.warning
                );

                ViewBag.SelectedCategories = selectedCategoryIds;

                await LoadCombosAsync();
                return View(cardDTO);

            }

            CardDTO existingCard;
            try
            {
                existingCard = await _serviceCard.FindByIdAsync(cardDTO.Id);
            }
            catch (Exception)
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                        "Card not found",
                        $"No card found with ID {cardDTO.Id}.",
                        SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }

            if(existingCard.AuctionStatus == "In Progress") 
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                        "Edit not allowed",
                        "This card cannot be edited because it is in an active auction.",
                        SweetAlertMessageType.warning
                );
                return RedirectToAction(nameof(Details), new { cardDTO.Id });
            }

            cardDTO.UserId = existingCard.UserId;
            cardDTO.IsActive = existingCard.IsActive;
            cardDTO.RegistrationDate = existingCard.RegistrationDate;

            List<CategoryCardDTO> categories = [];
            foreach (var catId in selectedCategoryIds)
            {
                categories.Add(new CategoryCardDTO
                {
                    CardId = cardDTO.Id,
                    CategoryId = catId,
                    IsActive = true,

                });
            }
            cardDTO.CategoryCard = categories;

            List<ImageDTO> images = [];
            foreach (var file in files)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                images.Add(new ImageDTO
                {
                    CardId = cardDTO.Id,
                    ImageData = ms.ToArray(),
                    IsActive = true
                });
            }
            cardDTO.Image = images;

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

            CardDTO card;
            try
            {
                card = await _serviceCard.FindByIdAsync(id.Value);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) 
        {
            CardDTO card;
            try
            {
                card = await _serviceCard.FindByIdAsync(id);
            }
            catch (Exception)
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

            card.IsActive = false;
            await _serviceCard.UpdateAsync(card);

            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Card deleted",
                "This card was desactived sucessfully.",
                SweetAlertMessageType.warning
                );

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id) 
        {
            CardDTO card;
            try
            {
                card = await _serviceCard.FindByIdAsync(id);
            }
            catch (Exception)
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
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "Action not allowed",
                    "This card cannot be modified because it is in an active auction.",
                    SweetAlertMessageType.warning
                    );
                return RedirectToAction(nameof(Details), new { id });
            }
            try
            {
                await _serviceCard.ToggleActiveAsync(id);
            } catch (Exception ex) 
            {
                TempData["Notification"] = SweetAlertHelper.CreateNotification(
                    "An error has occurred",
                    ex.Message,
                    SweetAlertMessageType.warning
                    );
                return RedirectToAction(nameof(Index));
            }


           TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Status updated",
                "The card status was updated successfully.",
                SweetAlertMessageType.success

                );

            return RedirectToAction(nameof(Index));
        }
    }


}
