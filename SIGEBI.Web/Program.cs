using SIGEBI.Web.Services;

namespace SIGEBI.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient("SIGEBIApi", client =>
            {
                client.BaseAddress = new Uri(
                    builder.Configuration["ApiSettings:BaseUrl"]
                    ?? "http://localhost:5148/api/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            builder.Services.AddScoped<IUsuarioApiService, UsuarioApiService>();
            builder.Services.AddScoped<IRecursoApiService, RecursoApiService>();
            builder.Services.AddScoped<IEjemplarApiService, EjemplarApiService>();
            builder.Services.AddScoped<IPrestamoApiService, PrestamoApiService>();
            builder.Services.AddScoped<IPenalizacionApiService, PenalizacionApiService>();
            builder.Services.AddScoped<INotificacionApiService, NotificacionApiService>();
            builder.Services.AddScoped<IAuthApiService, AuthApiService>();
            builder.Services.AddHttpContextAccessor();


            builder.Services.AddDistributedMemoryCache(); 

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}