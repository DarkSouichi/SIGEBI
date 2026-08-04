using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBI.Web.Models.Ejemplar;
using SIGEBI.Web.Models.Prestamo;
using SIGEBI.Web.Models.Usuario;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class PrestamoController : Controller
    {
        private readonly IPrestamoApiService _prestamoApiService;
        private readonly IUsuarioApiService _usuarioApiService;
        private readonly IEjemplarApiService _ejemplarApiService;

        public PrestamoController(
            IPrestamoApiService prestamoApiService,
            IUsuarioApiService usuarioApiService,
            IEjemplarApiService ejemplarApiService)
        {
            _prestamoApiService = prestamoApiService;
            _usuarioApiService = usuarioApiService;
            _ejemplarApiService = ejemplarApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _prestamoApiService.GetAll();
            if (result.isSuccess)
                return View(result.data ?? new List<PrestamoModel>());

            ModelState.AddModelError(string.Empty, result.message);
            return View(new List<PrestamoModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _prestamoApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data ?? new PrestamoModel());

            ModelState.AddModelError(string.Empty, result.message);
            return View(new PrestamoModel());
        }

        public async Task<IActionResult> Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            var model = new PrestamoCreateModel();
            await CargarListas(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrestamoCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarListas(model);
                return View(model);
            }

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _prestamoApiService.Create(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    await CargarListas(model);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor. Verifique que la API esté disponible.");
                await CargarListas(model);
                return View(model);
            }
            catch (TaskCanceledException)
            {
                ModelState.AddModelError(string.Empty, "La solicitud tardó demasiado. Verifique su conexión.");
                await CargarListas(model);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error inesperado: {ex.Message}");
                await CargarListas(model);
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            var result = await _prestamoApiService.GetById(id);
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new PrestamoEditModel());
            }

            var model = new PrestamoEditModel
            {
                id = result.data.prestamoId,
                usuarioId = result.data.usuarioId,
                ejemplarId = result.data.ejemplarId,
                fechaPrestamo = result.data.fechaPrestamo,
                fechaDevolucionEsperada = result.data.fechaDevolucionEsperada,
                fechaDevolucionReal = result.data.fechaDevolucionReal,
                estado = result.data.estado
            };

            await CargarListas(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrestamoEditModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarListas(model);
                return View(model);
            }

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _prestamoApiService.Update(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    await CargarListas(model);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor. Verifique que la API esté disponible.");
                await CargarListas(model);
                return View(model);
            }
            catch (TaskCanceledException)
            {
                ModelState.AddModelError(string.Empty, "La solicitud tardó demasiado. Verifique su conexión.");
                await CargarListas(model);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error inesperado: {ex.Message}");
                await CargarListas(model);
                return View(model);
            }
        }

        private async Task CargarListas(object model)
        {
            try
            {
                var usuariosResponse = await _usuarioApiService.GetAll();
                var usuarios = usuariosResponse.data ?? new List<UsuarioModel>();

                var ejemplaresResponse = await _ejemplarApiService.GetAll();
                var ejemplares = ejemplaresResponse.data ?? new List<EjemplarModel>();

                if (model is PrestamoCreateModel createModel)
                {
                    createModel.UsuariosList = usuarios.Select(u => new SelectListItem
                    {
                        Value = u.usuarioId.ToString(),
                        Text = $"{u.nombreCompleto} ({u.email})"
                    }).ToList();

                    createModel.EjemplaresList = ejemplares.Select(e => new SelectListItem
                    {
                        Value = e.ejemplarId.ToString(),
                        Text = $"Ejemplar #{e.ejemplarId} - Código: {e.codigoBarras} - Estado: {e.estado}"
                    }).ToList();
                }
                else if (model is PrestamoEditModel editModel)
                {
                    editModel.UsuariosList = usuarios.Select(u => new SelectListItem
                    {
                        Value = u.usuarioId.ToString(),
                        Text = $"{u.nombreCompleto} ({u.email})"
                    }).ToList();

                    editModel.EjemplaresList = ejemplares.Select(e => new SelectListItem
                    {
                        Value = e.ejemplarId.ToString(),
                        Text = $"Ejemplar #{e.ejemplarId} - Código: {e.codigoBarras} - Estado: {e.estado}"
                    }).ToList();
                }
            }
            catch
            {
            }
        }
    }
}