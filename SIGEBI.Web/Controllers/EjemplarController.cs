using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using SIGEBI.Web.Models.Ejemplar;
using SIGEBI.Web.Models.Recurso;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class EjemplarController : Controller
    {
        private readonly IEjemplarApiService _ejemplarApiService;
        private readonly IRecursoApiService _recursoApiService;

        public EjemplarController(IEjemplarApiService ejemplarApiService,
                                  IRecursoApiService recursoApiService)
        {
            _ejemplarApiService = ejemplarApiService;
            _recursoApiService = recursoApiService;
        }

        public async Task<IActionResult> Index(string estado)
        {
            var result = await _ejemplarApiService.GetAll();
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<EjemplarModel>());
            }

            var ejemplares = result.data ?? new List<EjemplarModel>();

            if (!string.IsNullOrEmpty(estado) && int.TryParse(estado, out var estadoInt))
            {
                ejemplares = ejemplares.Where(e => e.estado == estadoInt).ToList();
            }

            ViewBag.Estados = new Dictionary<int, string>
            {
                { 0, "Disponible" },
                { 1, "Prestado" },
                { 2, "Reservado" },
                { 3, "No Disponible" }
            };
            ViewBag.EstadoSeleccionado = estado;

            return View(ejemplares);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _ejemplarApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data ?? new EjemplarModel());

            ModelState.AddModelError(string.Empty, result.message);
            return View(new EjemplarModel());
        }

        public async Task<IActionResult> Create()
        {
            var model = new EjemplarCreateModel();
            await CargarRecursos(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EjemplarCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarRecursos(model);
                return View(model);
            }

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _ejemplarApiService.Create(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    await CargarRecursos(model);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error inesperado: {ex.Message}");
                await CargarRecursos(model);
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _ejemplarApiService.GetById(id);
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new EjemplarEditModel());
            }

            var model = new EjemplarEditModel
            {
                id = result.data.ejemplarId,
                codigoBarras = result.data.codigoBarras,
                estado = result.data.estado,
                recursoId = result.data.recursoId
            };
            await CargarRecursos(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EjemplarEditModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarRecursos(model);
                return View(model);
            }

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _ejemplarApiService.Update(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    await CargarRecursos(model);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error inesperado: {ex.Message}");
                await CargarRecursos(model);
                return View(model);
            }
        }

        private async Task CargarRecursos(object model)
        {
            try
            {
                var recursosResponse = await _recursoApiService.GetAll();
                var recursos = recursosResponse.data ?? new List<RecursoModel>();

                var selectList = recursos.Select(r => new SelectListItem
                {
                    Value = r.recursoId.ToString(),
                    Text = $"{r.titulo} - {r.autor}"
                }).ToList();

                if (model is EjemplarCreateModel createModel)
                {
                    createModel.RecursosList = selectList;
                }
                else if (model is EjemplarEditModel editModel)
                {
                    editModel.RecursosList = selectList;
                }
            }
            catch (Exception)
            {
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportarExcel(string estado)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
            {
                TempData["Error"] = "No tienes permisos para exportar.";
                return RedirectToAction("Index", "Home");
            }

            var result = await _ejemplarApiService.GetAll();
            if (!result.isSuccess || result.data == null || !result.data.Any())
            {
                TempData["Error"] = "No hay datos para exportar.";
                return RedirectToAction(nameof(Index));
            }

            var ejemplares = result.data;

            if (!string.IsNullOrEmpty(estado) && int.TryParse(estado, out var estadoInt))
            {
                ejemplares = ejemplares.Where(e => e.estado == estadoInt).ToList();
            }

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Ejemplares");

            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Código de Barras";
            worksheet.Cells[1, 3].Value = "Estado";
            worksheet.Cells[1, 4].Value = "Recurso (Libro)";

            using (var range = worksheet.Cells[1, 1, 1, 4])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            int row = 2;
            foreach (var item in ejemplares)
            {
                worksheet.Cells[row, 1].Value = item.ejemplarId;
                worksheet.Cells[row, 2].Value = item.codigoBarras;
                worksheet.Cells[row, 3].Value = item.EstadoTexto;
                worksheet.Cells[row, 4].Value = item.tituloRecurso;
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var fileBytes = package.GetAsByteArray();
            var fileName = $"Ejemplares_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}