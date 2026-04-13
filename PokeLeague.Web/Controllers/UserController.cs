using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;

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

        public async Task<IActionResult> Index()
        {
            var users = await _serviceUser.ListAsync();
            return View(users);
        }

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

        public async Task<IActionResult> Edit(int id)
        {
            var user= await _serviceUser.FindByIdAsync(id);

            if(user == null) 
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDTO user)
        {
            if (!ModelState.IsValid)
                return View(user);

            await _serviceUser.UpdateProfileAsync(user.Id, user.Username, user.Email);

            return RedirectToAction("Details", new { id = user.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleBlock(int id) 
        {
            await _serviceUser.ToggleBlockAsync(id);
            return RedirectToAction("Details" , new {id});
        }
    }
}
