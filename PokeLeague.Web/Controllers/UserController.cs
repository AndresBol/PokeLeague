using Microsoft.AspNetCore.Mvc;
using PokeLeague.Application.Services.Interfaces;

namespace PokeLeague.Web.Controllers
{
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
    }
}
