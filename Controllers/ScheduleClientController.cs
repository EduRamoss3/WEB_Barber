using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Enums;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Humanizer;
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
        public async Task<IActionResult> SelectBarber()
        {
            ParametersToPagination parameters = new(200, 1);
            var barbers = await _barberService.GetAllAsync(parameters, TokenJwt());
            ViewBag.Barbeiros = new SelectList(barbers.Objects, "Id", "Name");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SelectBarberWithHour(SelectModel model)
        {
            var session = HttpContext.Session.GetString("Email");
            if(session is null)
            {
                return View("Login", "Account");
            }

            var apiResponse = await _barberService.GetIndisponibleDateAsync(model.IdBarber, TokenJwt());
            if (apiResponse.OneObject is null)
            {
                return View("Error");
            }

            Horarios horarios = new();
            // Lista auxiliar para armazenar os horários a serem removidos
            var horariosToRemove = new List<DateTime>();

            foreach (var item in apiResponse.OneObject)
            {
                if (item.ToString("dd/MM") == model.Date.ToString("dd/MM"))
                {
                    foreach (var horario in horarios.HorariosList)
                    {
                        if (item.ToString("HH:mm") == horario.ToString("HH:mm"))
                        {
                            horariosToRemove.Add(horario);
                        }
                    }
                }
            }
            foreach (var horario in horariosToRemove)
            {
                horarios.HorariosList.Remove(horario);
            }
            var horarioSelectList = horarios.HorariosList
                .Select(h => new SelectListItem
                {
                    Value = h.ToString("HH:mm"),
                    Text = h.ToString("HH:mm")
                })
                .ToList();

            ViewBag.Horarios = horarioSelectList;
            return View("SelectBarberWithHour", model);
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(SelectModel model)
        {
            var dateParsed = DateTime.ParseExact(model.InitialDate, "HH:mm", CultureInfo.InvariantCulture);
            
            model.Date = new DateTime(model.Date.Year, model.Date.Month, model.Date.Day,
                                      dateParsed.Hour, dateParsed.Minute, 0);

            if (ModelState.IsValid)
            {
                var email = HttpContext.Session.GetString("Email");
                var idClient = await _clienteServices.GetIdByEmail(email, TokenJwt());

                SchedulesDTO schedulesDTO = new(model.IdBarber, idClient, model.TypeOfService, model.Date, false);
                var result = await _scheduleServices.AddAsync(schedulesDTO, TokenJwt());
                if (result == HttpStatusCode.Created)
                {
                    TempData["Success"] = "Agendamento realizado com sucesso!";
                    return View("Index");
                }
                TempData["Erro"] = "Erro ao realizar agendamento!";
                return View("Error");
            }
            ModelState.AddModelError("Erro", "Verifique todos os campos e tente novamente!");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> MySchedules()
        {
            var email = HttpContext.Session.GetString("Email");
            var idClient = await _clienteServices.GetIdByEmail(email, TokenJwt());

            if(idClient == 0)
            {
                return RedirectToAction("Login","Account");
            }

            var apiResponse = await _scheduleServices.GetByClientIdAsync(idClient, TokenJwt());       
            if(apiResponse.Objects is null)
            {
                return View("Error");
            }

            return View(apiResponse.Objects);
        }

    }
}
