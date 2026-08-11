using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SIGEBI.Web.Models.Usuario;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioApiService _usuarioApiService;
        private readonly IPenalizacionApiService _penalizacionApiService;

        public UsuarioController(IUsuarioApiService usuarioApiService,
                                 IPenalizacionApiService penalizacionApiService) 
        {
            _usuarioApiService = usuarioApiService;
            _penalizacionApiService = penalizacionApiService;
        }

        public async Task<IActionResult> Index(string rol, bool? activo)
        {
            var result = await _usuarioApiService.GetAll();
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<UsuarioModel>());
            }

            var usuarios = result.data ?? new List<UsuarioModel>();

            await EnriquecerConMora(usuarios);

            if (!string.IsNullOrEmpty(rol) && int.TryParse(rol, out var rolId))
                usuarios = usuarios.Where(u => u.rolId == rolId).ToList();
            if (activo.HasValue)
                usuarios = usuarios.Where(u => u.estaActivo == activo.Value).ToList();

            ViewBag.RolSeleccionado = rol;
            ViewBag.ActivoSeleccionado = activo?.ToString();

            return View(usuarios);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _usuarioApiService.GetById(id);
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new UsuarioModel());
            }

            var usuario = result.data;
            await EnriquecerConMora(usuario);

            return View(usuario);
        }

        private async Task EnriquecerConMora(List<UsuarioModel> usuarios)
        {
            if (usuarios == null || !usuarios.Any()) return;

            var penalizacionesResponse = await _penalizacionApiService.GetAll();
            if (penalizacionesResponse.isSuccess && penalizacionesResponse.data != null)
            {
                var moraPorUsuario = penalizacionesResponse.data
                    .Where(p => p.estado == "Activa")
                    .GroupBy(p => p.usuarioId)
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.monto));

                foreach (var item in usuarios)
                {
                    if (moraPorUsuario.TryGetValue(item.usuarioId, out var totalMora))
                        item.TotalMora = totalMora;
                    else
                        item.TotalMora = 0;
                }
            }
        }

        private async Task EnriquecerConMora(UsuarioModel usuario)
        {
            if (usuario == null) return;

            var penalizacionesResponse = await _penalizacionApiService.GetAll();
            if (penalizacionesResponse.isSuccess && penalizacionesResponse.data != null)
            {
                var totalMora = penalizacionesResponse.data
                    .Where(p => p.usuarioId == usuario.usuarioId && p.estado == "Activa")
                    .Sum(p => p.monto);
                usuario.TotalMora = totalMora;
            }
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

        [HttpGet]
        public async Task<IActionResult> ExportarExcel(string rol, bool? activo)
        {
            var rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Admin")
            {
                TempData["Error"] = "No tienes permisos para exportar.";
                return RedirectToAction("Index", "Home");
            }

            var result = await _usuarioApiService.GetAll();
            if (!result.isSuccess || result.data == null || !result.data.Any())
            {
                TempData["Error"] = "No hay datos para exportar.";
                return RedirectToAction(nameof(Index));
            }

            var usuarios = result.data;

            await EnriquecerConMora(usuarios);

            if (!string.IsNullOrEmpty(rol) && int.TryParse(rol, out var rolId))
                usuarios = usuarios.Where(u => u.rolId == rolId).ToList();
            if (activo.HasValue)
                usuarios = usuarios.Where(u => u.estaActivo == activo.Value).ToList();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Usuarios");

            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Nombre Completo";
            worksheet.Cells[1, 3].Value = "Email";
            worksheet.Cells[1, 4].Value = "Activo";
            worksheet.Cells[1, 5].Value = "Rol";
            worksheet.Cells[1, 6].Value = "Mora Acumulada"; 

            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            int row = 2;
            foreach (var item in usuarios)
            {
                worksheet.Cells[row, 1].Value = item.usuarioId;
                worksheet.Cells[row, 2].Value = item.nombreCompleto;
                worksheet.Cells[row, 3].Value = item.email;
                worksheet.Cells[row, 4].Value = item.estaActivo ? "Sí" : "No";
                worksheet.Cells[row, 5].Value = item.rolId == 1 ? "Admin" : "Usuario";
                worksheet.Cells[row, 6].Value = item.TotalMora; 
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var fileBytes = package.GetAsByteArray();
            var fileName = $"Usuarios_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}