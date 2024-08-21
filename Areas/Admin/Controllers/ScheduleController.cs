using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;

namespace Barber.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleServices _scheduleServices;
        private readonly IBarberService _barberService;
        private readonly IClienteService _clientService;
        string token = string.Empty;

        public ScheduleController(IScheduleServices scheduleServices, IBarberService barberService, IClienteService clientService)
        {
            _scheduleServices = scheduleServices;
            _barberService = barberService;
            _clientService = clientService;
        }
        private string TokenJwt()
        {
            if (HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
            {
                token = HttpContext.Request.Cookies["X-Access-Token"].ToString();
            }
            return token;
        }
        protected string GetUserRoleFromToken()
        {
           
            if (HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
            {
                var token = HttpContext.Request.Cookies["X-Access-Token"];
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                return roleClaim;
            }

            return null;
        }
        [Route("All")]
        [HttpGet]
        public async Task<IActionResult> All(ParametersToPagination parameters)
        {
            try
            {
                var userRole = GetUserRoleFromToken();
                if (userRole.Equals("Member"))
                {
                    return RedirectToAction("Index", "Home");
                }
                parameters.PageNumber = 1;
                parameters.PageSize = 30;
                var response = await _scheduleServices.GetAllAsync(parameters, TokenJwt());

                if (response.RequestUri.AbsolutePath.Contains("Account/Login")){
                    return Redirect($"{response.RequestUri.AbsolutePath}");
                }
               
                return View(response.Objects);
            }
            catch (HttpRequestException)
            {
                TempData["Erro"] = "Ocorreu um erro na requisição, por favor, contate o suporte.";
                return View("Error");
            }
            catch (SocketException)
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
                var response = await _scheduleServices.GetWithDataAsync(TokenJwt());

                if (response.RequestUri.AbsolutePath.Contains("Account/Login"))
                {
                    return Redirect($"{response.RequestUri.AbsolutePath}");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
                {
                    return RedirectToAction("Index", "Home");
                }
                return View(response.Objects);
            }
            catch (HttpRequestException)
            {
                TempData["Erro"] = "Ocorreu um erro na requisição, por favor, contate o suporte.";
                return View("Error");
            }
            catch (SocketException)
            {
                TempData["Erro"] = "Ocorreu um erro na requisição, por favor, contate o suporte.";
                return View("Error");
            }
        }


        [Route("Search")]
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult<List<SchedulesDTO>>> GetByClientId(int clientId)
        {
            try
            {
                var objectResponse = await _scheduleServices.GetByClientIdAsync(clientId, TokenJwt());
                if (objectResponse is null)
                {
                    return View("Error");
                }
                else if (objectResponse.StatusCode == HttpStatusCode.OK)
                {
                    if (objectResponse.Objects.Count > 0)
                    {
                        return View(objectResponse.Objects);
                    }
                }
                TempData["Error"] = objectResponse.Message;
                return View("Error");
            }
            catch (HttpRequestException)
            {
                TempData["Erro"] = "Erro interno, por favor, comunique ao suporte";
                return View("Error", TempData);
            }

        }
        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DeleteInformation(int id)
        {
            var schedules = await _scheduleServices.GetByIdAsync(id, TokenJwt());
            if(schedules.OneObject is not null)
            {
                return View(schedules.OneObject);
            }
            TempData["Error"] = "Erro ao encontrar o agendamento";
            return View("Error");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var service = await _scheduleServices.RemoveAsync(id, TokenJwt());
                if (service.Equals(HttpStatusCode.NotFound) || service.Equals(HttpStatusCode.BadRequest))
                {
                    TempData["Erro"] = "Agendamento não existe ou você não tem permissão necessária.";
                    return View("Error");
                }
                TempData["Success"] = "Agendamento removido com sucesso!";
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                TempData["Erro"] = "Erro na conexão, por favor consulte o suporte técnico.";
                return View("Error");
            }
        }
        public IActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ParametersToPagination parameters = new(200,1);
            var barbeiros = await _barberService.GetAllAsync(parameters, TokenJwt());
            var clientes = await _clientService.GetAllAsync(parameters, TokenJwt());

            ViewBag.Barbeiros = new SelectList(barbeiros.Objects,"Id","Name");
            ViewBag.Clientes = new SelectList(clientes.Objects, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(SchedulesDTO DTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _scheduleServices.AddAsync(DTO, TokenJwt());
                if (response.Equals(HttpStatusCode.Created))
                {
                    TempData["Success"] = "Agendamento adicionado com sucesso!";
                    return RedirectToAction("Index");
                }
                TempData["Erro"] = "Ocorreu um erro na requisição, por favor, contate o suporte.";
                return View(DTO);
            }
            ModelState.AddModelError("Error", "Verifique todos os campos e tente novamente!");
            return View(DTO);
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            ViewBag.Denied = "Acesso negado";
            return View();
        }
    }
}
