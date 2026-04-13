using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Web.Util;
using PokeLeague.Web.ViewModels;
using System.Security.Claims;

namespace PokeLeague.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServiceUser _serviceUser;

        public LoginController(IServiceUser serviceUser)
        {
            _serviceUser = serviceUser;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(ViewModelLogin viewModelLogin)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("<br>", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage)
                        ? "Unspecified validation error"
                        : e.ErrorMessage));

                ViewBag.Notification = SweetAlertHelper.CreateNotification(
                    "Validation Errors",
                    $"The form contains the following errors: {errors}",
                    SweetAlertMessageType.warning
                );

                return View("Index", viewModelLogin);
            }

            var user = await _serviceUser.LoginAsync(viewModelLogin.Email, viewModelLogin.Password);

            if (user == null)
            {
                ViewBag.Notification = SweetAlertHelper.CreateNotification(
                    "Access Denied",
                    "Invalid email or password.",
                    SweetAlertMessageType.warning
                );

                return View("Index", viewModelLogin);
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = false
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Welcome",
                $"Login successful. Hello, {user.Username}!",
                SweetAlertMessageType.success
            );

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync();

            TempData["Notification"] = SweetAlertHelper.CreateNotification(
                "Session Ended",
                "You have been logged out successfully.",
                SweetAlertMessageType.success
            );

            return RedirectToAction("Index", "Login");
        }

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
