using Barber.UI.Entities.Register;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        private IActionResult HandleUnauthorizedOrForbidden(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Forbidden)
                return RedirectToAction("AccessDenied", "Home", new { area = "" });

            if (statusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Account", new { area = "" });

            return null;
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
                var result = await _barberService.AddAsync(DTO, TokenJwt());
                var apiResponse = HandleUnauthorizedOrForbidden(result.StatusCode);
                if (apiResponse != null)
                {
                    return apiResponse;
                }
                if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created)
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


        public async Task<IActionResult> GetById(int idBarber)
        {
            if (ModelState.IsValid)
            {
                var result = await _barberService.GetById(idBarber, TokenJwt());
                var apiResponse = HandleUnauthorizedOrForbidden(result.StatusCode);
                if(apiResponse != null) { return apiResponse; }
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return View("GetById", result);
                }
                ModelState.AddModelError("Error", "Verifique todos os campos e tente novamente!");
            }
            return View();
        }
        public async Task<IActionResult> Barbers(ParametersToPagination parametersToPagination)
        {
            parametersToPagination.PageNumber = 1;
            parametersToPagination.PageSize = 200;
            var responseApi = await _barberService.GetAllAsync(parametersToPagination, TokenJwt());

            var apiResponse = HandleUnauthorizedOrForbidden(responseApi.StatusCode);
            if (apiResponse != null) { return apiResponse; }

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

            var responseApi = await _barberService.GetById(id, TokenJwt());

            var apiResponse = HandleUnauthorizedOrForbidden(responseApi.StatusCode);
            if (apiResponse != null) { return apiResponse; }
            if (responseApi.OneObject is not null)
            {
                return View(responseApi.OneObject);
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

                var apiResponse = HandleUnauthorizedOrForbidden(service.StatusCode);
                if (apiResponse != null) { return apiResponse; }
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
            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                if (apiResponse.OneObject is null)
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
