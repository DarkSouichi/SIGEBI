using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.Auth;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IAuthApiService _authApiService;

        public RegisterController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _authApiService.Register(model);
                if (!result.isSuccess)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return View(model);
                }

                TempData["Success"] = "Registro exitoso. Ahora puede iniciar sesión.";
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return View(model);
            }
        }
    }
}