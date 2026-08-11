using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Extensions;          
using SIGEBI.Web.Models.Ejemplar;     
using SIGEBI.Web.Models.Prestamo;
using SIGEBI.Web.Models.Recurso;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IRecursoApiService _recursoApiService;
        private readonly IPrestamoApiService _prestamoApiService;
        private readonly IEjemplarApiService _ejemplarApiService; 

        public CarritoController(IRecursoApiService recursoApiService,
                                 IPrestamoApiService prestamoApiService,
                                 IEjemplarApiService ejemplarApiService) 
        {
            _recursoApiService = recursoApiService;
            _prestamoApiService = prestamoApiService;
            _ejemplarApiService = ejemplarApiService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(int recursoId)
        {
            var userId = HttpContext.Session.GetInt32("UsuarioId");
            if (!userId.HasValue)
            {
                TempData["Error"] = "Debes iniciar sesión para agregar al carrito.";
                return RedirectToAction("Login", "Auth");
            }

            var recursoResult = await _recursoApiService.GetById(recursoId);
            if (!recursoResult.isSuccess || recursoResult.data == null || recursoResult.data.ejemplaresDisponibles <= 0)
            {
                TempData["Error"] = "No hay ejemplares disponibles de este libro.";
                return RedirectToAction("Details", "Recurso", new { id = recursoId });
            }

            var carrito = HttpContext.Session.GetObjectFromJson<List<int>>("Carrito") ?? new List<int>();

            if (!carrito.Contains(recursoId))
            {
                carrito.Add(recursoId);
                HttpContext.Session.SetObjectAsJson("Carrito", carrito);
                TempData["Success"] = "Libro agregado al carrito.";
            }
            else
            {
                TempData["Info"] = "El libro ya está en el carrito.";
            }

            return RedirectToAction("Details", "Recurso", new { id = recursoId });
        }

        public async Task<IActionResult> Index()
        {
            var carrito = HttpContext.Session.GetObjectFromJson<List<int>>("Carrito") ?? new List<int>();
            if (!carrito.Any())
            {
                ViewBag.Mensaje = "El carrito está vacío.";
                return View(new List<RecursoModel>());
            }

            var recursos = new List<RecursoModel>();
            foreach (var id in carrito)
            {
                var result = await _recursoApiService.GetById(id);
                if (result.isSuccess && result.data != null)
                    recursos.Add(result.data);
            }

            return View(recursos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Solicitar()
        {
            var userId = HttpContext.Session.GetInt32("UsuarioId");
            if (!userId.HasValue)
            {
                TempData["Error"] = "Debes iniciar sesión para solicitar préstamos.";
                return RedirectToAction("Login", "Auth");
            }

            var carrito = HttpContext.Session.GetObjectFromJson<List<int>>("Carrito") ?? new List<int>();
            if (!carrito.Any())
            {
                TempData["Error"] = "El carrito está vacío.";
                return RedirectToAction("Index");
            }

            var successCount = 0;
            var errors = new List<string>();

            foreach (var recursoId in carrito)
            {
                var ejemplaresResponse = await _ejemplarApiService.GetAll();
                var ejemplar = ejemplaresResponse?.data?.FirstOrDefault(e => e.recursoId == recursoId && e.estado == 0);
                if (ejemplar == null)
                {
                    errors.Add($"No hay ejemplares disponibles para el recurso #{recursoId}.");
                    continue;
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
                    successCount++;
                else
                    errors.Add(result.message ?? "Error al crear el préstamo.");
            }

            HttpContext.Session.Remove("Carrito");

            if (successCount > 0)
                TempData["Success"] = $"Se solicitaron {successCount} préstamos. {errors.Count} errores.";
            else
                TempData["Error"] = "No se pudo solicitar ningún préstamo. " + string.Join(" ", errors);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Quitar(int recursoId)
        {
            var carrito = HttpContext.Session.GetObjectFromJson<List<int>>("Carrito") ?? new List<int>();
            carrito.Remove(recursoId);
            HttpContext.Session.SetObjectAsJson("Carrito", carrito);
            return RedirectToAction("Index");
        }
    }
}