using SIGEBI.Web.Services;

namespace SIGEBI.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // Registrar HttpClient con la URL base de la API
            builder.Services.AddHttpClient("SIGEBIApi", client =>
            {
                client.BaseAddress = new Uri(
                    builder.Configuration["ApiSettings:BaseUrl"] ??
                    "http://localhost:5148/api/");
            });

            // Registrar los servicios de consumo de API
            builder.Services.AddScoped<IUsuarioApiService, UsuarioApiService>();
            builder.Services.AddScoped<IRecursoApiService, RecursoApiService>();
            builder.Services.AddScoped<IPrestamoApiService, PrestamoApiService>();
            builder.Services.AddScoped<IPenalizacionApiService, PenalizacionApiService>();
            builder.Services.AddScoped<INotificacionApiService, NotificacionApiService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}