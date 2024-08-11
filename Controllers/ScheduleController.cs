using Barber.UI.Entities;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace Barber.UI.Controllers
{
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
        public IActionResult Index()
        {
            return View();
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
                if(objectResponse is null)
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
            catch(HttpRequestException)
            {
                TempData["Erro"] = "Erro interno, por favor, comunique ao suporte";
                return View("Error",TempData);
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
            catch(Exception e)
            {
                TempData["Erro"] = "Erro na conexão, por favor consulte o suporte técnico.";
                return View("Error");
            }  
        }
        [HttpGet]
        public async Task<ActionResult<List<SchedulesDTO>>> ListSchedules(ParametersToPagination parameters)
        {
            var response = await _scheduleServices.GetAllAsync(parameters,TokenJwt());
            if(response.StatusCode == HttpStatusCode.OK && response.Objects is not null)
            {
                return View(response.Objects);
            }
            TempData["Erro"] = "Ocorreu um erro na requisição, por favor, contate o suporte.";
            return View("Error");
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
