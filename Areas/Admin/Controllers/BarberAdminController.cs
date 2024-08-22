using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Register;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;
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
        public async Task<ActionResult<List<BarberDTO>>> Barbers(ParametersToPagination parametersToPagination)
        {
            parametersToPagination.PageNumber = 1;
            parametersToPagination.PageSize = 200;
            var responseApi = await _barberService.GetAllAsync(parametersToPagination,TokenJwt());
            if (responseApi.StatusCode == HttpStatusCode.OK)
            {
                return View(responseApi.Objects);
            }
            TempData["Erro"] = "Ocorreu algum erro durante a sua requisição";
            return View("Error");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteInformation(int id)
        {
          
            var barber = await _barberService.GetById(id, TokenJwt());
            if (barber.OneObject is not null)
            {
                return View(barber.OneObject);
            }
            TempData["Error"] = "Erro ao encontrar o barbeiro";
            return View("Error");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var service = await _barberService.RemoveByIdAsync(id, TokenJwt());
                if (service.Equals(HttpStatusCode.NotFound) || service.Equals(HttpStatusCode.BadRequest))
                {
                    TempData["Erro"] = "Agendamento não existe ou você não tem permissão necessária.";
                    return View("Error");
                }
                TempData["Success"] = "Barbeiro removido com sucesso!";
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                TempData["Erro"] = "Erro na conexão, por favor consulte o suporte técnico.";
                return View("Error");
            }
        }
        //[HttpGet]
        //public async Task<IActionResult> (int id)
        //{

        //}
        //[HttpPost]
        //public async Task<ActionResult<BarberDTO>> Edit(BarberDTO barberDTO)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var apiResponse = await _barberService.UpdateAsync(barberDTO, barberDTO.Id, TokenJwt());
        //        if (apiResponse.Equals(HttpStatusCode.OK) || apiResponse.Equals(HttpStatusCode.Created))
        //        {
        //            TempData["Success"] = "Agendamento atualizado com sucesso!";
        //            return RedirectToAction("Index");
        //        }
        //        TempData["Erro"] = "Erro na requisição";
        //        return View("Error");

        //    }
        //    ModelState.AddModelError("Erro", "Verifique todos os campos e tente novamente");
        //    return View(barberDTO);
        //}

        public async Task<IActionResult> Details(int id)
        {
            var apiResponse = await _barberService.GetById(id, TokenJwt());
            if(apiResponse.StatusCode == HttpStatusCode.OK)
            {
                if(apiResponse.OneObject is null)
                {
                    TempData["Erro"] = "Ocorreu um erro durante a sua requisição.";
                    return View("Error");
                }
                return View(apiResponse.OneObject);
            }
            TempData["Erro"] = "Erro ao localizar barbeiro.";
            return View("Error");

        }

        public IActionResult Error()
        {
            return View();
        }
        
    }
}
