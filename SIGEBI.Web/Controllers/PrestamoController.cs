using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
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

        // ===================== ACCIONES PRINCIPALES =====================

        public async Task<IActionResult> Index(string estado)
        {
            var result = await _prestamoApiService.GetAll();
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<PrestamoModel>());
            }

            var prestamos = result.data ?? new List<PrestamoModel>();

            if (!string.IsNullOrEmpty(estado))
            {
                prestamos = prestamos.Where(p => p.estado == estado).ToList();
            }

            ViewBag.Estados = new List<string> { "Pendiente", "Activo", "Devuelto", "Vencido" };
            ViewBag.EstadoSeleccionado = estado;

            return View(prestamos);
        }


        public async Task<IActionResult> MisPrestamos()
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

            var result = await _prestamoApiService.GetAll();
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<PrestamoModel>());
            }

            var prestamos = result.data ?? new List<PrestamoModel>();
            prestamos = prestamos.Where(p => p.usuarioId == userId.Value).ToList();

            return View(prestamos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _prestamoApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data ?? new PrestamoModel());

            ModelState.AddModelError(string.Empty, result.message);
            return View(new PrestamoModel());
        }

        // ===================== ACCIONES DE ADMINISTRACIÓN =====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
            {
                TempData["Error"] = "No tienes permisos para aprobar préstamos.";
                return RedirectToAction("Index", "Home");
            }

            var result = await _prestamoApiService.GetById(id);
            if (!result.isSuccess)
            {
                TempData["Error"] = "Préstamo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var model = new PrestamoEditModel
            {
                id = result.data.prestamoId,
                usuarioId = result.data.usuarioId,
                ejemplarId = result.data.ejemplarId,
                fechaPrestamo = result.data.fechaPrestamo,
                fechaDevolucionEsperada = result.data.fechaDevolucionEsperada,
                fechaDevolucionReal = result.data.fechaDevolucionReal,
                estado = "Activo",
                changeDate = DateTime.Now,
                changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1
            };

            var updateResult = await _prestamoApiService.Update(model);
            if (updateResult.isSuccess)
            {
                TempData["Success"] = "Préstamo aprobado correctamente. El inventario se ha actualizado.";
            }
            else
            {
                TempData["Error"] = updateResult.message ?? "Error al aprobar el préstamo.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            var result = await _prestamoApiService.GetById(id);
            if (!result.isSuccess)
            {
                TempData["Error"] = "Préstamo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var model = new PrestamoEditModel
            {
                id = result.data.prestamoId,
                usuarioId = result.data.usuarioId,
                ejemplarId = result.data.ejemplarId,
                fechaPrestamo = result.data.fechaPrestamo,
                fechaDevolucionEsperada = result.data.fechaDevolucionEsperada,
                fechaDevolucionReal = DateTime.Now,
                estado = "Devuelto",
                changeDate = DateTime.Now,
                changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1
            };

            var updateResult = await _prestamoApiService.Update(model);
            if (updateResult.isSuccess)
            {
                TempData["Success"] = "Devolución registrada correctamente. El inventario se ha actualizado.";
            }
            else
            {
                TempData["Error"] = updateResult.message ?? "Error al registrar la devolución.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Solicitar(int recursoId)
        {
            var userId = HttpContext.Session.GetInt32("UsuarioId");
            if (!userId.HasValue)
            {
                TempData["Error"] = "Debes iniciar sesión para solicitar un préstamo.";
                return RedirectToAction("Login", "Auth");
            }

            var ejemplaresResponse = await _ejemplarApiService.GetAll();
            var ejemplar = ejemplaresResponse?.data?.FirstOrDefault(e => e.recursoId == recursoId && e.estado == 0);

            if (ejemplar == null)
            {
                TempData["Error"] = "No hay ejemplares disponibles de este libro.";
                return RedirectToAction("Details", "Recurso", new { id = recursoId });
            }

            var model = new PrestamoCreateModel
            {
                usuarioId = userId.Value,
                ejemplarId = ejemplar.ejemplarId,
                fechaPrestamo = DateTime.Now,
                fechaDevolucionEsperada = DateTime.Now.AddDays(7),
                estado = "Pendiente",
                changeDate = DateTime.Now,
                changeUser = userId.Value
            };

            var result = await _prestamoApiService.Create(model);
            if (result.isSuccess)
            {
                TempData["Success"] = "Solicitud de préstamo enviada. Espera la aprobación del bibliotecario.";
            }
            else
            {
                TempData["Error"] = result.message ?? "Error al solicitar el préstamo.";
            }

            return RedirectToAction("Details", "Recurso", new { id = recursoId });
        }

        // ===================== CRUD (CREAR, EDITAR) =====================

        public async Task<IActionResult> Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            var model = new PrestamoCreateModel
            {
                fechaPrestamo = DateTime.Now
            };

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

            var result = await _prestamoApiService.GetAll();
            if (!result.isSuccess || result.data == null || !result.data.Any())
            {
                TempData["Error"] = "No hay datos para exportar.";
                return RedirectToAction(nameof(Index));
            }

            var prestamos = result.data;

            if (!string.IsNullOrEmpty(estado))
            {
                prestamos = prestamos.Where(p => p.estado == estado).ToList();
            }

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Préstamos");

            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Usuario";
            worksheet.Cells[1, 3].Value = "Ejemplar";
            worksheet.Cells[1, 4].Value = "Fecha Préstamo";
            worksheet.Cells[1, 5].Value = "Fecha Devolución Esperada";
            worksheet.Cells[1, 6].Value = "Fecha Devolución Real";
            worksheet.Cells[1, 7].Value = "Estado";

            using (var range = worksheet.Cells[1, 1, 1, 7])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            int row = 2;
            foreach (var item in prestamos)
            {
                worksheet.Cells[row, 1].Value = item.prestamoId;
                worksheet.Cells[row, 2].Value = item.nombreUsuario;
                worksheet.Cells[row, 3].Value = item.codigoEjemplar;
                worksheet.Cells[row, 4].Value = item.fechaPrestamo.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cells[row, 5].Value = item.fechaDevolucionEsperada.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cells[row, 6].Value = item.fechaDevolucionReal?.ToString("dd/MM/yyyy HH:mm") ?? "-";
                worksheet.Cells[row, 7].Value = item.estado;
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var fileBytes = package.GetAsByteArray();
            var fileName = $"Prestamos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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
                // Si falla, no mostrar error para no bloquear la vista
            }
        }
    }
}