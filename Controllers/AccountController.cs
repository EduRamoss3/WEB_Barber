using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Net.Sockets;
using System.Text.Json;

namespace Barber.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticate _authenticate;
        private string token = string.Empty;
        public AccountController(IAuthenticate authenticate)
        {
            _authenticate = authenticate;
        }
        public IActionResult Login()
        {
            return View();
        }
        private string TokenJwt()
        {
            if (HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
            {
                token = HttpContext.Request.Cookies["X-Access-Token"].ToString();
            }
            return token;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
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


                    HttpContext.Session.SetString("Email", model.Email);
                    HttpContext.Session.SetString("Logged", "true");
                    HttpContext.Session.SetString("Role", result.Role);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Falha ao fazer login. Verifique suas credenciais.");
                HttpContext.Session.SetString("Logged", "false");
                return View();
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("Request", "Erro de comunicação, por favor, contate o suporte.");
                return View();
            }
            catch (SocketException)
            {
                ModelState.AddModelError("Request", "Erro de comunicação, por favor, contate o suporte.");
                return View();
            }

        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var result = await _authenticate.Logout(TokenJwt());
            if (result == System.Net.HttpStatusCode.OK)
            {
                HttpContext.Session.Remove("Logged");
                HttpContext.Session.Remove("Email");
                HttpContext.Session.Remove("Role");
                Response.Cookies.Delete("X-Access-Token");
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Erro = "Erro ao realizar logout";
            return View("Erro");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("Registro", "Verifique todos os campos e tente novamente!");
                    return View();
                }
                var result = await _authenticate.Register(model);
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = result.Content.ReadAsStringAsync();
                    return View(ViewBag.Error);
                }
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("Request", "Erro de comunicação, por favor, contate o suporte.");
                return View();
            }
            catch (SocketException)
            {
                ModelState.AddModelError("Request", "Erro de comunicação, por favor, contate o suporte.");
                return View();
            }
           
        }
    }
}
