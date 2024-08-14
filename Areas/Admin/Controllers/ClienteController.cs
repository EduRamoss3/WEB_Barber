using Barber.UI.Entities;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Barber.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
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
    }
}
