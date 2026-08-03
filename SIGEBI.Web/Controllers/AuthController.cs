using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.Auth;
using SIGEBI.Web.Services;

namespace SIGEBI.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiService _authApiService;

        public AuthController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authApiService.Login(model.Email, model.Password);
            if (result.isSuccess)
            {
                HttpContext.Session.SetString("Token", result.token);
                HttpContext.Session.SetString("NombreCompleto", result.nombreCompleto);
                HttpContext.Session.SetString("Rol", result.rol);
                HttpContext.Session.SetInt32("UsuarioId", result.usuarioId);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, result.message);
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}