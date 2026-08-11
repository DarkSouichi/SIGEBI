using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
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

        public async Task<IActionResult> Index(string categoria, string vista)
        {
            if (string.IsNullOrEmpty(vista))
            {
                vista = HttpContext.Session.GetString("VistaRecursos") ?? "lista";
            }
            else
            {
                HttpContext.Session.SetString("VistaRecursos", vista);
            }

            var result = await _recursoApiService.GetAll();
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<RecursoModel>());
            }

            var recursos = result.data ?? new List<RecursoModel>();

            ViewBag.Categorias = recursos.Select(r => r.categoria).Distinct().OrderBy(c => c).ToList();
            ViewBag.CategoriaSeleccionada = categoria;
            ViewBag.VistaActual = vista; 

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

        [HttpGet]
        public async Task<IActionResult> ExportarExcel(string categoria)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
            {
                TempData["Error"] = "No tienes permisos para exportar.";
                return RedirectToAction("Index", "Home");
            }

            var result = await _recursoApiService.GetAll();
            if (!result.isSuccess || result.data == null || !result.data.Any())
            {
                TempData["Error"] = "No hay datos para exportar.";
                return RedirectToAction(nameof(Index));
            }

            var recursos = result.data;

            if (!string.IsNullOrEmpty(categoria))
            {
                recursos = recursos.Where(r => r.categoria == categoria).ToList();
            }

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Recursos");

            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Título";
            worksheet.Cells[1, 3].Value = "Autor";
            worksheet.Cells[1, 4].Value = "ISBN";
            worksheet.Cells[1, 5].Value = "Categoría";
            worksheet.Cells[1, 6].Value = "Total Ejemplares";
            worksheet.Cells[1, 7].Value = "Disponibles";
            worksheet.Cells[1, 8].Value = "Año de Lanzamiento";
            worksheet.Cells[1, 9].Value = "Descripción";

            using (var range = worksheet.Cells[1, 1, 1, 9])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            int row = 2;
            foreach (var item in recursos)
            {
                worksheet.Cells[row, 1].Value = item.recursoId;
                worksheet.Cells[row, 2].Value = item.titulo;
                worksheet.Cells[row, 3].Value = item.autor;
                worksheet.Cells[row, 4].Value = item.isbn;
                worksheet.Cells[row, 5].Value = item.categoria;
                worksheet.Cells[row, 6].Value = item.totalEjemplares;
                worksheet.Cells[row, 7].Value = item.ejemplaresDisponibles;
                worksheet.Cells[row, 8].Value = item.fechaLanzamiento?.Year.ToString() ?? "";
                worksheet.Cells[row, 9].Value = item.descripcion;
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var fileBytes = package.GetAsByteArray();
            var fileName = $"Recursos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}