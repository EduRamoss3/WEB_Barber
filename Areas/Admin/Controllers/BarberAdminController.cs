using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Register;
using Barber.UI.Entities.Responses;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Net;

namespace Barber.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BarberAdminController : Controller
    {
        private readonly IBarberService _barberService;
        string token = string.Empty;

        public BarberAdminController(IBarberService barberService)
        {
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(BarberRegisterDTO DTO)
        {
            if (ModelState.IsValid)
            {
               var result = await _barberService.AddAsync(DTO,TokenJwt());
               if(result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created)
               {
                    TempData["Success"] = "Adicionado com sucesso!";
                    return View("Index");
               }
                ViewBag.Erro = "Erro ao cadastrar barbeiro!";
                return View("Error");
            }
            ModelState.AddModelError("Erro", "Verifique todos os campos e tente novamente!");

            return View();
        }
        public IActionResult GetById()
        {
            return View();   
        }


        public async Task<ActionResult<ObjectResponse<BarberDTO>>> GetById(int idBarber)
        {
            if (ModelState.IsValid)
            {
                var result = await _barberService.GetById(idBarber, TokenJwt());
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return View("GetById", result);
                }
                ModelState.AddModelError("Error", "Verifique todos os campos e tente novamente!");
            }
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
