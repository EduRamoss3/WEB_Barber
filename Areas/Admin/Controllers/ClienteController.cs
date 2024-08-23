using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Register;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Barber.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;
        private const string TokenCookieName = "X-Access-Token";


        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        private string GetTokenFromCookie()
        {
            return HttpContext.Request.Cookies.ContainsKey(TokenCookieName) ? HttpContext.Request.Cookies[TokenCookieName] : string.Empty;
        }

        private IActionResult HandleUnauthorizedOrForbidden(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Forbidden)
                return RedirectToAction("AccessDenied","Home", new { area = "" });

            if (statusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Account", new { area = "" });

            return null;
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
                var result = await _clienteService.AddAsync(clientRegisterDTO, GetTokenFromCookie());

                if (result)
                {
                    TempData["Success"] = "Cliente registrado com sucesso!";
                    return RedirectToAction("Manager");
                }

                TempData["Erro"] = "Erro ao registrar o cliente.";
                return View("Error");
            }

            ModelState.AddModelError("Erro", "Verifique todos os campos e tente novamente!");
            return View(clientRegisterDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Manager()
        {
            var parameters = new ParametersToPagination(200, 1);
            var apiResponse = await _clienteService.GetAllAsync(parameters, GetTokenFromCookie());

            var unauthorizedResponse = HandleUnauthorizedOrForbidden(apiResponse.StatusCode);
            if (unauthorizedResponse != null)
                return unauthorizedResponse;

            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                return View(apiResponse.Objects);
            }

            TempData["Erro"] = "Ocorreu um erro durante a sua requisição!";
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var apiResponse = await _clienteService.GetByIdAsync(id, GetTokenFromCookie());

            var unauthorizedResponse = HandleUnauthorizedOrForbidden(apiResponse.StatusCode);
            if (unauthorizedResponse != null)
                return unauthorizedResponse;

            if (apiResponse.OneObject != null)
            {
                return View(apiResponse.OneObject);
            }

            TempData["Erro"] = "Erro ao localizar cliente.";
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteInformation(int id)
        {
            var response = await _clienteService.GetByIdAsync(id, GetTokenFromCookie());

            var unauthorizedResponse = HandleUnauthorizedOrForbidden(response.StatusCode);
            if (unauthorizedResponse != null)
                return unauthorizedResponse;

            if (response.OneObject != null)
            {
                return View(response.OneObject);
            }

            TempData["Erro"] = "Erro ao encontrar o cliente.";
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _clienteService.RemoveAsync(id, GetTokenFromCookie());

                if (!result)
                {
                    TempData["Erro"] = "Cliente não existe ou você não tem permissão necessária.";
                    return View("Error");
                }

                TempData["Success"] = "Cliente removido com sucesso!";
                return RedirectToAction("Manager");
            }
            catch (Exception)
            {
                TempData["Erro"] = "Erro na conexão, por favor consulte o suporte técnico.";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _clienteService.GetByIdAsync(id, GetTokenFromCookie());

            var unauthorizedResponse = HandleUnauthorizedOrForbidden(response.StatusCode);
            if (unauthorizedResponse != null)
                return unauthorizedResponse;

            if (response.OneObject != null)
            {
                return View(response.OneObject);
            }

            TempData["Erro"] = "Cliente não encontrado!";
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClientDTO clientDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _clienteService.UpdateAsync(clientDTO, clientDTO.Id, GetTokenFromCookie());

                if (result)
                {
                    TempData["Success"] = "Cliente atualizado com sucesso!";
                    return RedirectToAction("Manager");
                }

                TempData["Erro"] = "Erro ao atualizar o cliente.";
                return View("Error");
            }

            ModelState.AddModelError("Erro", "Verifique todos os campos e tente novamente");
            return View(clientDTO);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
