using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Web.Util;
using System.Security.Claims;

namespace PokeLeague.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IServiceUser _serviceUser;

        public UserController(IServiceUser serviceUser)
        {
            _serviceUser = serviceUser;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _serviceUser.ListAsync();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var user = await _serviceUser.FindByIdAsync(id.Value);

            if(user == null) {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var user= await _serviceUser.FindByIdAsync(id);

            if(user == null) 
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(UserDTO user)
        {
            if (!ModelState.IsValid)
                return View(user);

            await _serviceUser.UpdateProfileAsync(user.Id, user.Username, user.Email);

            return RedirectToAction("Details", new { id = user.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleBlock(int id) 
        {
            await _serviceUser.ToggleBlockAsync(id);
            return RedirectToAction("Details" , new {id});
        }

        public async Task<IActionResult> EditProfile()
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _serviceUser.FindByIdAsync(currentUserId);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserDTO user, string? newPassword, string? confirmPassword)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (user.Id != currentUserId)
            {
                return Forbid();
            }

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if (newPassword != confirmPassword)
                {
                    ModelState.AddModelError("confirmPassword", "Passwords do not match.");
                    return View(user);
                }

                if (newPassword.Length < 6 || newPassword.Length > 15)
                {
                    ModelState.AddModelError("newPassword", "Password must be between 6 and 15 characters.");
                    return View(user);
                }
            }

            await _serviceUser.UpdateProfileAsync(user.Id, user.Username, user.Email);

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                await _serviceUser.UpdatePasswordAsync(user.Id, newPassword);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Profile Updated",
                "Your profile was updated successfully. Please log in again.",
                SweetAlertMessageType.success
            );

            return RedirectToAction("Index", "Login");
        }
    }
}
