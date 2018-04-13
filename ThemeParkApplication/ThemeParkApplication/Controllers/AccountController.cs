using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThemeParkApplication.Models;

namespace ThemeParkApplication.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            ViewBag.Title = "Login Page";
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login vm)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }
    }
}