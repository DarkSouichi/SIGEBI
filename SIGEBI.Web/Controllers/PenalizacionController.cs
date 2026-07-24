using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.Penalizacion;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class PenalizacionController : Controller
    {
        private readonly IPenalizacionApiService _penalizacionApiService;

        public PenalizacionController(IPenalizacionApiService penalizacionApiService)
        {
            _penalizacionApiService = penalizacionApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _penalizacionApiService.GetAll();
            if (result.isSuccess)
                return View(result.data);
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<PenalizacionModel>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _penalizacionApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data);
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new PenalizacionModel());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PenalizacionCreateModel model)
        {
            try
            {
                var result = await _penalizacionApiService.Create(model);
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
            var result = await _penalizacionApiService.GetById(id);
            if (result.isSuccess)
            {
                var editModel = new PenalizacionEditModel
                {
                    penalizacionId = result.data.penalizacionId,
                    usuarioId = result.data.usuarioId,
                    prestamoId = result.data.prestamoId,
                    monto = result.data.monto,
                    estado = result.data.estado
                };
                return View(editModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new PenalizacionEditModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PenalizacionEditModel model)
        {
            try
            {
                var result = await _penalizacionApiService.Update(model);
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