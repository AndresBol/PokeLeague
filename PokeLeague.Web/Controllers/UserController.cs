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
        private readonly IServiceRole _serviceRole;

        public UserController(IServiceUser serviceUser, IServiceRole serviceRole)
        {
            _serviceUser = serviceUser;
            _serviceRole = serviceRole;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _serviceUser.ListAsync();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _serviceRole.ListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(UserDTO user, string password)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                ModelState.AddModelError("Username", "Username is required.");

            if (string.IsNullOrWhiteSpace(user.Email))
                ModelState.AddModelError("Email", "Email is required.");

            if (string.IsNullOrWhiteSpace(password))
                ModelState.AddModelError("password", "Password is required.");
            else if (password.Length < 6 || password.Length > 15)
                ModelState.AddModelError("password", "Password must be between 6 and 15 characters.");

            if (user.Role == null || user.Role.Id == 0)
                ModelState.AddModelError("Role.Id", "Please select a role.");

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _serviceRole.ListAsync();
                ViewBag.Notification = SweetAlertHelper.CreateNotification(
                    "Validation errors",
                    "Please complete all required fields.",
                    SweetAlertMessageType.warning
                );
                return View(user);
            }

            user.IsActive = true;
            user.IsBlocked = false;
            user.SignupDate = DateOnly.FromDateTime(DateTime.Now);

            var id = await _serviceUser.CreateAsync(user, password);

            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "User created",
                "The user was created successfully.",
                SweetAlertMessageType.success
            );

            return RedirectToAction(nameof(Details), new { id });
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
