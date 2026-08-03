using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBI.Web.Models.Penalizacion;
using SIGEBI.Web.Models.Prestamo;
using SIGEBI.Web.Models.Usuario;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class PenalizacionController : Controller
    {
        private readonly IPenalizacionApiService _penalizacionApiService;
        private readonly IUsuarioApiService _usuarioApiService;
        private readonly IPrestamoApiService _prestamoApiService;

        public PenalizacionController(IPenalizacionApiService penalizacionApiService,
                                      IUsuarioApiService usuarioApiService,
                                      IPrestamoApiService prestamoApiService)
        {
            _penalizacionApiService = penalizacionApiService;
            _usuarioApiService = usuarioApiService;
            _prestamoApiService = prestamoApiService;
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

        public async Task<IActionResult> Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            var model = new PenalizacionCreateModel();
            await CargarListas(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PenalizacionCreateModel model)
        {
            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _penalizacionApiService.Create(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    await CargarListas(model);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await CargarListas(model);
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            var result = await _penalizacionApiService.GetById(id);
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new PenalizacionEditModel());
            }

            var model = new PenalizacionEditModel
            {
                id = result.data.penalizacionId,
                usuarioId = result.data.usuarioId,
                prestamoId = result.data.prestamoId,
                monto = result.data.monto,
                estado = result.data.estado,
                fechaEmision = result.data.fechaEmision
            };

            await CargarListas(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PenalizacionEditModel model)
        {
            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _penalizacionApiService.Update(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    Console.WriteLine($"Error al actualizar penalización: {result.message}");
                    await CargarListas(model);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción en Edit POST: {ex.Message}");
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                await CargarListas(model);
                return View(model);
            }
        }

        private async Task CargarListas(object model)
        {
            var usuariosResponse = await _usuarioApiService.GetAll();
            var usuarios = usuariosResponse.data ?? new List<UsuarioModel>();

            var prestamosResponse = await _prestamoApiService.GetAll();
            var prestamos = prestamosResponse.data ?? new List<PrestamoModel>();

            if (model is PenalizacionCreateModel createModel)
            {
                createModel.UsuariosList = usuarios.Select(u => new SelectListItem
                {
                    Value = u.usuarioId.ToString(),
                    Text = $"{u.nombreCompleto} ({u.email})"
                }).ToList();

                createModel.PrestamosList = prestamos.Select(p => new SelectListItem
                {
                    Value = p.prestamoId.ToString(),
                    Text = $"Préstamo #{p.prestamoId} - Usuario: {p.usuarioId} - Estado: {p.estado}"
                }).ToList();
            }
            else if (model is PenalizacionEditModel editModel)
            {
                editModel.UsuariosList = usuarios.Select(u => new SelectListItem
                {
                    Value = u.usuarioId.ToString(),
                    Text = $"{u.nombreCompleto} ({u.email})"
                }).ToList();

                editModel.PrestamosList = prestamos.Select(p => new SelectListItem
                {
                    Value = p.prestamoId.ToString(),
                    Text = $"Préstamo #{p.prestamoId} - Usuario: {p.usuarioId} - Estado: {p.estado}"
                }).ToList();
            }
        }
    }
}