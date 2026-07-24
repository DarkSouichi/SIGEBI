using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.Notificacion;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class NotificacionController : Controller
    {
        private readonly INotificacionApiService _notificacionApiService;

        public NotificacionController(INotificacionApiService notificacionApiService)
        {
            _notificacionApiService = notificacionApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _notificacionApiService.GetAll();
            if (result.isSuccess)
                return View(result.data);
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new List<NotificacionModel>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _notificacionApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data);
            else
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new NotificacionModel());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificacionCreateModel model)
        {
            try
            {
                var result = await _notificacionApiService.Create(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}