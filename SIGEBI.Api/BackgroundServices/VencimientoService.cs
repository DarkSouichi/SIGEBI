using SIGEBI.Domain.Entities.Loans;
using SIGEBI.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace SIGEBI.Api.BackgroundServices
{
    public class VencimientoService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VencimientoService> _logger;

        public VencimientoService(IServiceProvider serviceProvider,
                                   ILogger<VencimientoService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("VencimientoService iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var ahora = DateTime.Now;
                var proxima8AM = ahora.Date.AddHours(8);

                if (ahora > proxima8AM)
                    proxima8AM = proxima8AM.AddDays(1);

                var espera = proxima8AM - ahora;
                _logger.LogInformation($"VencimientoService: próxima ejecución en {espera.TotalHours:F1} horas.");
                await Task.Delay(espera, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                    await VerificarVencimientos();
            }
        }

        private async Task VerificarVencimientos()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

                var hoy = DateTime.Now.Date;

                var prestamosVencidos = await context.Prestamos
                    .Where(p => p.Estado == EstadoPrestamo.Activo
                             && p.FechaDevolucionEsperada.Date < hoy)
                    .ToListAsync();

                foreach (var prestamo in prestamosVencidos)
                {
                    prestamo.Estado = EstadoPrestamo.Vencido;

                    var diasRetraso = (hoy - prestamo.FechaDevolucionEsperada.Date).Days;
                    var monto = diasRetraso * 100;

                    var tienePenalizacion = await context.Penalizaciones
                        .AnyAsync(p => p.PrestamoId == prestamo.Id && p.Estado == "Activa");

                    if (!tienePenalizacion)
                    {
                        context.Penalizaciones.Add(new Domain.Entities.Penalties.Penalizacion
                        {
                            UsuarioId = prestamo.UsuarioId,
                            PrestamoId = prestamo.Id,
                            Monto = monto,
                            Estado = "Activa",
                            FechaEmision = DateTime.Now,
                            CreadoEn = DateTime.Now,
                            CreadoPor = "Sistema"
                        });
                    }
                }

                var en3Dias = hoy.AddDays(3);
                var prestamosProximos = await context.Prestamos
                    .Where(p => p.Estado == EstadoPrestamo.Activo
                             && p.FechaDevolucionEsperada.Date >= hoy
                             && p.FechaDevolucionEsperada.Date <= en3Dias)
                    .ToListAsync();

                if (prestamosProximos.Any())
                {
                    _logger.LogInformation($"VencimientoService: {prestamosProximos.Count} préstamos próximos a vencer.");
                }

                await context.SaveChangesAsync();
                _logger.LogInformation("VencimientoService: verificación completada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en VencimientoService al verificar vencimientos.");
            }
        }
    }
}