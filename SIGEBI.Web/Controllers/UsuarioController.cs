using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.Usuario;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioApiService _usuarioApiService;

        public UsuarioController(IUsuarioApiService usuarioApiService)
        {
            _usuarioApiService = usuarioApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _usuarioApiService.GetAll();
            if (result.isSuccess)
                return View(result.data ?? new List<UsuarioModel>());

            ModelState.AddModelError(string.Empty, result.message);
            return View(new List<UsuarioModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _usuarioApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data ?? new UsuarioModel());

            ModelState.AddModelError(string.Empty, result.message);
            return View(new UsuarioModel());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateModel model)
        {
            model.estaActivo = true;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _usuarioApiService.Create(model);
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
            var result = await _usuarioApiService.GetById(id);
            if (result.isSuccess)
            {
                var editModel = new UsuarioEditModel
                {
                    usuarioId = result.data.usuarioId,
                    nombreCompleto = result.data.nombreCompleto,
                    email = result.data.email,
                    estaActivo = result.data.estaActivo,
                    rolId = result.data.rolId
                };
                return View(editModel);
            }

            ModelState.AddModelError(string.Empty, result.message);
            return View(new UsuarioEditModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _usuarioApiService.Update(model);
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