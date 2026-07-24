using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.Prestamo;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class PrestamoController : Controller
    {
        private readonly IPrestamoApiService _prestamoApiService;

        public PrestamoController(IPrestamoApiService prestamoApiService)
        {
            _prestamoApiService = prestamoApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _prestamoApiService.GetAll();
            if (result.isSuccess)
                return View(result.data);
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<PrestamoModel>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _prestamoApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data);
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new PrestamoModel());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrestamoCreateModel model)
        {
            try
            {
                var result = await _prestamoApiService.Create(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _prestamoApiService.GetById(id);
            if (result.isSuccess)
            {
                var editModel = new PrestamoEditModel
                {
                    prestamoId = result.data.prestamoId,
                    usuarioId = result.data.usuarioId,
                    ejemplarId = result.data.ejemplarId,
                    fechaDevolucionEsperada = result.data.fechaDevolucionEsperada,
                    estado = result.data.estado
                };
                return View(editModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new PrestamoEditModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrestamoEditModel model)
        {
            try
            {
                var result = await _prestamoApiService.Update(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}