using Barber.UI.Areas.Admin.Models;
using Barber.UI.Entities.DTO;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace Barber.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleServices _scheduleServices;
        private readonly IBarberService _barberService;
        private readonly IClienteService _clientService;
        private const string TokenCookieName = "X-Access-Token";


        public ScheduleController(IScheduleServices scheduleServices, IBarberService barberService, IClienteService clientService)
        {
            _scheduleServices = scheduleServices;
            _barberService = barberService;
            _clientService = clientService;
        }

        private string GetTokenFromCookie()
        {
            return HttpContext.Request.Cookies.ContainsKey(TokenCookieName) ? HttpContext.Request.Cookies[TokenCookieName] : string.Empty;
        }

        private string GetUserRoleFromToken()
        {
            var token = GetTokenFromCookie();
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                return roleClaim;
            }
            return null;
        }

        private async Task<BarbersAndClientsModel> FetchBarbersAndClientsAsync()
        {
            ParametersToPagination parameters = new(200, 1);
            var barbeiros = await _barberService.GetAllAsync(parameters, GetTokenFromCookie());
            var clientes = await _clientService.GetAllAsync(parameters, GetTokenFromCookie());
            return new BarbersAndClientsModel(barbeiros.Objects, clientes.Objects);
        }

        private IActionResult HandleUnauthorizedOrForbidden(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Forbidden)
                return RedirectToAction("AccessDenied", "Home", new { area = "" });

            if (statusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Account", new { area = "" });

            return null;
        }

        [Route("All")]
        [HttpGet]
        public async Task<IActionResult> All(ParametersToPagination parameters)
        {
            try
            {
                if (GetUserRoleFromToken() == "Member")
                {
                    return RedirectToAction("Index", "Home");
                }

                parameters.PageNumber = 1;
                parameters.PageSize = 30;
                var response = await _scheduleServices.GetAllAsync(parameters, GetTokenFromCookie());

                var unauthorizedResponse = HandleUnauthorizedOrForbidden(response.StatusCode);
                if (unauthorizedResponse != null)
                    return unauthorizedResponse;

                return View(response.Objects);
            }
            catch (HttpRequestException)
            {
                TempData["Erro"] = "Ocorreu um erro na requisição, por favor, contate o suporte.";
                return View("Error");
            }
        }

        [Route("Index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _scheduleServices.GetWithDataAsync(GetTokenFromCookie());

                var unauthorizedResponse = HandleUnauthorizedOrForbidden(response.StatusCode);
                if (unauthorizedResponse != null)
                    return unauthorizedResponse;

                return View(response.Objects);
            }
            catch (HttpRequestException)
            {
                TempData["Erro"] = "Ocorreu um erro na requisição, por favor, contate o suporte.";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            try
            {
                var response = await _scheduleServices.GetByClientIdAsync(clientId, GetTokenFromCookie());

                var unauthorizedResponse = HandleUnauthorizedOrForbidden(response.StatusCode);
                if (unauthorizedResponse != null)
                    return unauthorizedResponse;

                if (response.Objects.Any())
                {
                    return View(response.Objects);
                }

                TempData["Erro"] = response.Message ?? "Erro ao buscar agendamentos.";
                return View("Error");
            }
            catch (HttpRequestException)
            {
                TempData["Erro"] = "Erro interno, por favor, comunique ao suporte";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteInformation(int id)
        {
            var response = await _scheduleServices.GetByIdAsync(id, GetTokenFromCookie());

            var unauthorizedResponse = HandleUnauthorizedOrForbidden(response.StatusCode);
            if (unauthorizedResponse != null)
                return unauthorizedResponse;

            if (response.OneObject != null)
            {
                return View(response.OneObject);
            }

            TempData["Erro"] = "Erro ao encontrar o agendamento";
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _scheduleServices.RemoveAsync(id, GetTokenFromCookie());

                var unauthorizedResponse = HandleUnauthorizedOrForbidden(response);
                if (unauthorizedResponse != null)
                    return unauthorizedResponse;

                if (response == HttpStatusCode.OK)
                {
                    TempData["Success"] = "Agendamento removido com sucesso!";
                    return RedirectToAction("Index");
                }

                TempData["Erro"] = "Erro ao remover o agendamento.";
                return View("Error");
            }
            catch (Exception)
            {
                TempData["Erro"] = "Erro na conexão, por favor consulte o suporte técnico.";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var items = await FetchBarbersAndClientsAsync();
            ViewBag.Barbeiros = new SelectList(items.Barbers, "Id", "Name");
            ViewBag.Clientes = new SelectList(items.Clients, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(SchedulesDTO dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _scheduleServices.AddAsync(dto, GetTokenFromCookie());

                var unauthorizedResponse = HandleUnauthorizedOrForbidden(response);
                if (unauthorizedResponse != null)
                    return unauthorizedResponse;

                if (response == HttpStatusCode.Created)
                {
                    TempData["Success"] = "Agendamento adicionado com sucesso!";
                    return RedirectToAction("Index");
                }

                TempData["Erro"] = "Erro ao adicionar o agendamento.";
                return View(dto);
            }

            ModelState.AddModelError("Erro", "Verifique todos os campos e tente novamente");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int idSchedule)
        {
            var response = await _scheduleServices.GetByIdAsync(idSchedule, GetTokenFromCookie());

            var unauthorizedResponse = HandleUnauthorizedOrForbidden(response.StatusCode);
            if (unauthorizedResponse != null)
                return unauthorizedResponse;

            if (response.OneObject == null)
            {
                TempData["Erro"] = "Agendamento não encontrado!";
                return View("Error");
            }

            var items = await FetchBarbersAndClientsAsync();
            ViewBag.Barbeiros = new SelectList(items.Barbers, "Id", "Name");
            ViewBag.Clientes = new SelectList(items.Clients, "Id", "Name");
            return View(response.OneObject);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SchedulesDTO dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _scheduleServices.UpdateAsync(dto, dto.Id, GetTokenFromCookie());

                var unauthorizedResponse = HandleUnauthorizedOrForbidden(response);
                if (unauthorizedResponse != null)
                    return unauthorizedResponse;

                if (response == HttpStatusCode.OK || response == HttpStatusCode.Created)
                {
                    TempData["Success"] = "Agendamento atualizado com sucesso!";
                    return RedirectToAction("Index");
                }

                TempData["Erro"] = "Erro ao atualizar o agendamento.";
                return View("Error");
            }

            ModelState.AddModelError("Erro", "Verifique todos os campos e tente novamente");
            return View(dto);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            ViewBag.Denied = "Acesso negado";
            return View();
        }
    }
}
