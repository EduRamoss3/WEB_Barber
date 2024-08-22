using Barber.UI.Entities.Register;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace Barber.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;
        private string token = string.Empty;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }
        private string TokenJwt()
        {
            if (HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
            {
                token = HttpContext.Request.Cookies["X-Access-Token"].ToString();
            }
            return token;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(ClientRegisterDTO clientRegisterDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _clienteService.AddAsync(clientRegisterDTO);
                if (result)
                {
                    TempData["Success"] = "Cliente registrado com sucesso!";
                    return View("Index");
                }
                return View("Error");
            }
            ModelState.AddModelError("Error", "Verifique todos os campos e tente novamente!");
            return View(clientRegisterDTO);
        }
        [HttpGet]
        public async Task<IActionResult> Manager()
        {
            ParametersToPagination parameters = new(200, 1);
            var apiResponse = await _clienteService.GetAllAsync(parameters, TokenJwt());
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View(apiResponse.Objects);
            }
            TempData["Erro"] = "Ocorreu um erro durante a sua requisição!";
            return View("Error");
        }
     
    }
}
