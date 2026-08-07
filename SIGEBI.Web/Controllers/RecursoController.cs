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

        public async Task<IActionResult> Index(string categoria)
        {
            var result = await _recursoApiService.GetAll();
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<RecursoModel>());
            }

            var recursos = result.data ?? new List<RecursoModel>();

            ViewBag.Categorias = recursos.Select(r => r.categoria).Distinct().OrderBy(c => c).ToList();
            ViewBag.CategoriaSeleccionada = categoria;

            if (!string.IsNullOrEmpty(categoria))
            {
                recursos = recursos.Where(r => r.categoria == categoria).ToList();
            }

            return View(recursos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _recursoApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data ?? new RecursoModel());

            ModelState.AddModelError(string.Empty, result.message);
            return View(new RecursoModel());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecursoCreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                if (model.fechaLanzamiento.HasValue)
                {
                    model.FechaLanzamientoApi = new DateTime(model.fechaLanzamiento.Value, 1, 1);
                }
                else
                {
                    model.FechaLanzamientoApi = null;
                }

                var result = await _recursoApiService.Create(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor. Verifique que la API esté disponible.");
                return View(model);
            }
            catch (TaskCanceledException)
            {
                ModelState.AddModelError(string.Empty, "La solicitud tardó demasiado. Verifique su conexión.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error inesperado: {ex.Message}");
                return View(model);
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
                    categoria = result.data.categoria,
                    descripcion = result.data.descripcion,
                    fechaLanzamiento = result.data.fechaLanzamiento?.Year
                };
                return View(editModel);
            }

            ModelState.AddModelError(string.Empty, result.message);
            return View(new RecursoEditModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RecursoEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                if (model.fechaLanzamiento.HasValue)
                {
                    model.FechaLanzamientoApi = new DateTime(model.fechaLanzamiento.Value, 1, 1);
                }
                else
                {
                    model.FechaLanzamientoApi = null;
                }

                var result = await _recursoApiService.Update(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor. Verifique que la API esté disponible.");
                return View(model);
            }
            catch (TaskCanceledException)
            {
                ModelState.AddModelError(string.Empty, "La solicitud tardó demasiado. Verifique su conexión.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error inesperado: {ex.Message}");
                return View(model);
            }
        }
    }
}