using Barber.UI.Entities;
using Barber.UI.Entities.Responses;
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
        public IActionResult Error()
        {
            return View();
        }
    }
}
