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
                return View(result.data);

            ModelState.AddModelError(string.Empty, result.message);
            return View(new List<PrestamoModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _prestamoApiService.GetById(id);

            if (result.isSuccess)
                return View(result.data);

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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                await CargarListas(model);
                return View(model);
            }
        }

        private async Task CargarListas(object model)
        {
            var usuariosResponse = await _usuarioApiService.GetAll();
            var usuarios = usuariosResponse.data ?? new List<UsuarioModel>();

            Console.WriteLine($"Usuarios obtenidos: {usuarios.Count}");

            var ejemplaresResponse = await _ejemplarApiService.GetAll();

            Console.WriteLine($"Ejemplares obtenidos: {ejemplaresResponse?.data?.Count ?? 0}");
            Console.WriteLine($"isSuccess: {ejemplaresResponse?.isSuccess}");
            Console.WriteLine($"message: {ejemplaresResponse?.message}");

            var ejemplares = ejemplaresResponse?.data ?? new List<EjemplarModel>();

            foreach (var e in ejemplares.Take(5))
            {
                Console.WriteLine($"Ejemplar: {e.ejemplarId} - {e.codigoBarras} - {e.estado}");
            }

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

                Console.WriteLine($"EjemplaresList cargada con {createModel.EjemplaresList.Count} elementos.");
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

                Console.WriteLine($"EjemplaresList cargada con {editModel.EjemplaresList.Count} elementos.");
            }
        }
    }
}