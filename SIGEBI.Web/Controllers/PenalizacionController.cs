using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
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

        // ===================== ACCIONES PRINCIPALES =====================

        public async Task<IActionResult> Index(string estado)
        {
            var result = await _penalizacionApiService.GetAll();
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<PenalizacionModel>());
            }

            var penalizaciones = result.data ?? new List<PenalizacionModel>();

            if (!string.IsNullOrEmpty(estado))
            {
                penalizaciones = penalizaciones.Where(p => p.estado == estado).ToList();
            }

            ViewBag.Estados = new List<string> { "Activa", "Resuelta", "Cancelada" };
            ViewBag.EstadoSeleccionado = estado;

            return View(penalizaciones);
        }

        public async Task<IActionResult> MisPenalizaciones()
        {
            var userId = HttpContext.Session.GetInt32("UsuarioId");
            var esAdmin = HttpContext.Session.GetString("Rol") == "Admin";

            if (esAdmin)
                return RedirectToAction("Index");

            if (!userId.HasValue)
            {
                TempData["Error"] = "Debes iniciar sesión.";
                return RedirectToAction("Login", "Auth");
            }

            var result = await _penalizacionApiService.GetAll();
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<PenalizacionModel>());
            }

            var penalizaciones = result.data ?? new List<PenalizacionModel>();
            penalizaciones = penalizaciones.Where(p => p.usuarioId == userId.Value).ToList();

            return View(penalizaciones);
        }

        // ===================== EXPORTAR A EXCEL =====================

        [HttpGet]
        public async Task<IActionResult> ExportarExcel(string estado)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
            {
                TempData["Error"] = "No tienes permisos para exportar.";
                return RedirectToAction("Index", "Home");
            }

            var result = await _penalizacionApiService.GetAll();
            if (!result.isSuccess || result.data == null || !result.data.Any())
            {
                TempData["Error"] = "No hay datos para exportar.";
                return RedirectToAction(nameof(Index));
            }

            var penalizaciones = result.data;

            if (!string.IsNullOrEmpty(estado))
            {
                penalizaciones = penalizaciones.Where(p => p.estado == estado).ToList();
            }

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Penalizaciones");

            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Usuario";
            worksheet.Cells[1, 3].Value = "Préstamo";
            worksheet.Cells[1, 4].Value = "Monto";
            worksheet.Cells[1, 5].Value = "Estado";
            worksheet.Cells[1, 6].Value = "Fecha Emisión";

            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            int row = 2;
            foreach (var item in penalizaciones)
            {
                worksheet.Cells[row, 1].Value = item.penalizacionId;
                worksheet.Cells[row, 2].Value = item.nombreUsuario;
                worksheet.Cells[row, 3].Value = item.prestamoInfo;
                worksheet.Cells[row, 4].Value = item.monto;
                worksheet.Cells[row, 5].Value = item.estado;
                worksheet.Cells[row, 6].Value = item.fechaEmision.ToString("dd/MM/yyyy HH:mm");
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var fileBytes = package.GetAsByteArray();
            var fileName = $"Penalizaciones_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

     

        public async Task<IActionResult> Details(int id)
        {
            var result = await _penalizacionApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data ?? new PenalizacionModel());

            ModelState.AddModelError(string.Empty, result.message);
            return View(new PenalizacionModel());
        }

        // ===================== CRUD (CREAR, EDITAR) =====================

        public async Task<IActionResult> Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            var model = new PenalizacionCreateModel
            {
                fechaEmision = DateTime.Now
            };

            await CargarListas(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PenalizacionCreateModel model)
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

                var result = await _penalizacionApiService.Create(model);
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
            if (!ModelState.IsValid)
            {
                await CargarListas(model);
                return View(model);
            }

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _penalizacionApiService.Update(model);
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

        // ===================== MÉTODOS PRIVADOS =====================

        private async Task CargarListas(object model)
        {
            try
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
                        Text = $"Préstamo #{p.prestamoId} - {p.codigoEjemplar} - {p.estado}"
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
                        Text = $"Préstamo #{p.prestamoId} - {p.codigoEjemplar} - {p.estado}"
                    }).ToList();
                }
            }
            catch
            {
                // Si falla, no mostrar error para no bloquear la vista
            }
        }
    }
}