using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.Recurso;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class RecursoController : Controller
    {
        private readonly IRecursoApiService _recursoApiService;

        public RecursoController(IRecursoApiService recursoApiService)
        {
            _recursoApiService = recursoApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _recursoApiService.GetAll();
            if (result.isSuccess)
                return View(result.data);
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<RecursoModel>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _recursoApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data);
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new RecursoModel());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecursoCreateModel model)
        {
            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _recursoApiService.Create(model);
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
            var result = await _recursoApiService.GetById(id);
            if (result.isSuccess)
            {
                var editModel = new RecursoEditModel
                {
                    id = result.data.recursoId,
                    titulo = result.data.titulo,
                    autor = result.data.autor,
                    isbn = result.data.isbn,
                    categoria = result.data.categoria
                };
                return View(editModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new RecursoEditModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RecursoEditModel model)
        {
            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _recursoApiService.Update(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return View(model);
            }
        }
    }
}