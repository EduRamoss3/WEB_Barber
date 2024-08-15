using Barber.UI.Entities;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Barber.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleServices _scheduleServices;
        string token = string.Empty;

        public ScheduleController(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
        }
        private string TokenJwt()
        {
            if (HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
            {
                token = HttpContext.Request.Cookies["X-Access-Token"].ToString();
            }
            return token;
        }
        [Route("Index")]
        [HttpGet]
        public async Task<IActionResult> Index(ParametersToPagination parameters)
        {
            try
            {
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
                return View("Index");

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
        public IActionResult Add()
        {
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
                    return RedirectToAction("ListSchedules");
                }
                TempData["Erro"] = "Ocorreu um erro na requisição, por favor, contate o suporte.";
                return View("Error");
            }
            ModelState.AddModelError("Error", "Verifique todos os campos e tente novamente!");
            return View(DTO);
        }
    }
}
