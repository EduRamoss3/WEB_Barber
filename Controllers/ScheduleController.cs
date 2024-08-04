using Barber.UI.Entities;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Barber.UI.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleServices _scheduleServices;
        public ScheduleController(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
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
            var item = await _scheduleServices.GetByClientIdAsync(clientId);
            return View(item);
        }
    }
}
