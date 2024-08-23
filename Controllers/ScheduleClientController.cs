using Barber.UI.Entities.DTO;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;
using System.Globalization;
using System.Net;

namespace Barber.UI.Controllers
{
    public class ScheduleClientController : Controller
    {
        private readonly IScheduleServices _scheduleServices;
        private readonly IClienteService _clienteServices;
        private readonly IBarberService _barberService;

        private string token = string.Empty;

        public ScheduleClientController(IScheduleServices scheduleServices, IClienteService clienteService, IBarberService barberService)
        {
            _scheduleServices = scheduleServices;
            _clienteServices = clienteService;
            _barberService = barberService;
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
        public async Task<IActionResult> BookAppointment()
        {
            ParametersToPagination parameters = new(200, 1);
            var barbers = await _barberService.GetAllAsync(parameters,TokenJwt());
            ViewBag.Barbeiros = new SelectList(barbers.Objects, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BookAppointment(SchedulesDTO scheduleDTO)
        {
            var id =  await _clienteServices.GetIdByEmail(HttpContext.Session.GetString("Email"), TokenJwt());
            if (id == 0)
            {
                ModelState.AddModelError("Erro", "Verifique se fez o login e tente novamente!");
                return RedirectToAction("Login", "Account");
            }
            scheduleDTO.IdClient = id;
            
            if (ModelState.IsValid)
            {
                bool isValidDate = await _scheduleServices.GetByDateDisponible(scheduleDTO.IdBarber, scheduleDTO.DateSchedule, TokenJwt());
                if (isValidDate)
                {
                    var responseStatusCode = await _scheduleServices.AddAsync(scheduleDTO, TokenJwt());
                    if(responseStatusCode == System.Net.HttpStatusCode.Forbidden || responseStatusCode == HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("AcessDenied", "Home");
                    }
                    if (responseStatusCode == System.Net.HttpStatusCode.Created)
                    {
                        TempData["Success"] = "Seu agendamento foi realizado com sucesso!";
                        return RedirectToAction("Index", "ScheduleClient");
                    }
                    TempData["Erro"] = "Ocorreu um erro na sua requisição, por favor, contate o suporte.";
                    return View("Error");
                }
                ModelState.AddModelError("Erro", "Data inválida, escolha outra data ou outro barbeiro!");

                ParametersToPagination parameters = new(200, 1);
                var barbers = await _barberService.GetAllAsync(parameters, TokenJwt());
                ViewBag.Barbeiros = new SelectList(barbers.Objects, "Id", "Name");
                return View(scheduleDTO);
            }
            ModelState.AddModelError("Erro", "Verifique todos os campos e tente novamente!");
            return View(scheduleDTO);
        }
       
    }
}
