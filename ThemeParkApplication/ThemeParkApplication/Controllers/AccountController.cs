using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThemeParkApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ThemeParkApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly themeparkdbContext _context;

        public AccountController(themeparkdbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            ViewBag.Title = "Login Page";
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Login(Login vm)
        {
            if (ModelState.IsValid)
            {
                var themeparkdbContext = _context.Profile.Include(p => p.Employee);
                ProfilesController profiles = new ProfilesController(_context);

                    if(profiles.checkPassword(vm.Email, vm.Password))
                {
                    return RedirectToAction("Index", "Home/About");
                }
                    else
                    return RedirectToAction("Index", "Home");


                //ModelState.AddModelError("", "Invalid Login Attempt.");
                //  return View(vm);

            }
            return RedirectToAction("Index", "Home");
           // return View(vm);
        }
    }
}