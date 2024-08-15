using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Barber.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticate _authenticate;
        public AccountController(IAuthenticate authenticate)
        {
            _authenticate = authenticate;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            bool isLogged = false;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Falha ao fazer login. Verifique suas credenciais.");
                return View(model);
            }
            var result = await _authenticate.Authenticates(model);
            if (result is not null)
            {
                Response.Cookies.Append("X-Access-Token", result.Token, new CookieOptions()
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.Expiration,
                });

                isLogged = true;
                HttpContext.Session.SetString("Logged", isLogged.ToString());

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Falha ao fazer login. Verifique suas credenciais.");
            HttpContext.Session.SetString("Logged", isLogged.ToString());
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Registro", "Verifique todos os campos e tente novamente!");
                return View();
            }
            var result = await _authenticate.Register(model);
            if (result.IsSuccessStatusCode)
            {
                return View("Index", "Home");
            }
            else
            {
                ViewBag.Error = result.Content.ReadAsStringAsync();
                return View(ViewBag.Error);
            }
        }
    }
}
