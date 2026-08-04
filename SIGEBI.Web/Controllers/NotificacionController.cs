using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBI.Web.Models.Notificacion;
using SIGEBI.Web.Models.Usuario;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class NotificacionController : Controller
    {
        private readonly INotificacionApiService _notificacionApiService;
        private readonly IUsuarioApiService _usuarioApiService;

        public NotificacionController(INotificacionApiService notificacionApiService,
                                      IUsuarioApiService usuarioApiService)
        {
            _notificacionApiService = notificacionApiService;
            _usuarioApiService = usuarioApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _notificacionApiService.GetAll();
            if (result.isSuccess)
                return View(result.data ?? new List<NotificacionModel>());

            ModelState.AddModelError(string.Empty, result.message);
            return View(new List<NotificacionModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _notificacionApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data ?? new NotificacionModel());

            ModelState.AddModelError(string.Empty, result.message);
            return View(new NotificacionModel());
        }

        public async Task<IActionResult> Create()
        {
            var model = new NotificacionCreateModel();
            await CargarUsuarios(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificacionCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarUsuarios(model);
                return View(model);
            }

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _notificacionApiService.Create(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    await CargarUsuarios(model);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error inesperado: {ex.Message}");
                await CargarUsuarios(model);
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _notificacionApiService.GetById(id);
            if (!result.isSuccess)
            {
                ModelState.AddModelError(string.Empty, result.message);
                return View(new NotificacionEditModel());
            }

            var model = new NotificacionEditModel
            {
                id = result.data.notificacionId,
                usuarioId = result.data.usuarioId,
                tipo = result.data.tipo,
                mensaje = result.data.mensaje,
                canal = result.data.canal,
                enviadoEn = result.data.enviadoEn
            };
            await CargarUsuarios(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NotificacionEditModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarUsuarios(model);
                return View(model);
            }

            try
            {
                model.changeDate = DateTime.Now;
                model.changeUser = HttpContext.Session.GetInt32("UsuarioId") ?? 1;

                var result = await _notificacionApiService.Update(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    await CargarUsuarios(model);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error inesperado: {ex.Message}");
                await CargarUsuarios(model);
                return View(model);
            }
        }

        private async Task CargarUsuarios(object model)
        {
            try
            {
                var usuariosResponse = await _usuarioApiService.GetAll();
                var usuarios = usuariosResponse.data ?? new List<UsuarioModel>();

                var selectList = usuarios.Select(u => new SelectListItem
                {
                    Value = u.usuarioId.ToString(),
                    Text = $"{u.nombreCompleto} ({u.email})"
                }).ToList();

                if (model is NotificacionCreateModel createModel)
                {
                    createModel.UsuariosList = selectList;
                }
                else if (model is NotificacionEditModel editModel)
                {
                    editModel.UsuariosList = selectList;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}