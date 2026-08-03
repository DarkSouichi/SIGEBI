using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Index()
        {
            var result = await _ejemplarApiService.GetAll();
            if (result.isSuccess)
                return View(result.data);
            else
                return View(new List<EjemplarModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _ejemplarApiService.GetById(id);
            if (result.isSuccess)
                return View(result.data);
            else
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
            catch
            {
                await CargarRecursos(model);
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _ejemplarApiService.GetById(id);
            if (!result.isSuccess)
                return View(new EjemplarEditModel());

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
            catch
            {
                await CargarRecursos(model);
                return View(model);
            }
        }


        private async Task CargarRecursos(object model)
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
    }
}