using FinancieraCreditoApi.Data;
using FinancieraCreditoApi.DTOs;
using FinancieraCreditoApi.Models;
using FinancieraCreditoApi.Services.Interfaces;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


namespace FinancieraCreditoApi.Services
{
    public class RespuestaCreditoService : IRespuestaCreditoService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RespuestaCreditoService> _logger;

        public RespuestaCreditoService(AppDbContext context, ILogger<RespuestaCreditoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RespuestaCreditoFinanciera> ProcesarRespuestaAsync(RespuestaCreditoRequest request)
        {
            var solicitud = await _context.SolicitudCredito
                .FirstOrDefaultAsync(s => s.NumeroSolicitud == request.NumeroSolicitud.Trim());
            var estadoUpper = request.Estado.Trim().ToUpper();

            if (solicitud == null)
                throw new KeyNotFoundException($"No existe la solicitud {request.NumeroSolicitud}");

            if (estadoUpper == "EN_PROCESO")
            {
                var parametro = await _context.ParametrosReconsulta
                    .FirstOrDefaultAsync(p => p.IdFinanciera == solicitud.IdFinanciera && p.Activo);

                int minutos = parametro?.MinutosReconsulta ?? 1440;
                var proximaConsulta = DateTime.UtcNow.AddMinutes(minutos);

                // 1. Cuando el estado es EN_PROCESO
                _logger.LogWarning(
                    "EN_PROCESO - Solicitud: {Numero} | Reintento en {Minutos} minutos → {Fecha}",
                    request.NumeroSolicitud, minutos, proximaConsulta.ToString("dd/MM/yyyy HH:mm"));

                var notificacionProceso = new NotificacionAsesor
                {
                    IdAsesor = solicitud.IdAsesor,
                    IdSolicitud = solicitud.Id,
                    Mensaje = $"Solicitud {request.NumeroSolicitud} EN PROCESO. " +
                              $"Volver a consultar el {proximaConsulta:dd/MM/yyyy HH:mm} " +
                              $"(aproximadamente en {minutos} minutos).",
                    Fecha = DateTime.UtcNow
                };

                _context.NotificacionAsesor.Add(notificacionProceso);
                await _context.SaveChangesAsync();

                // Para devolver objeto temporal
                return new RespuestaCreditoFinanciera
                {
                    NumeroSolicitud = request.NumeroSolicitud,
                    Estado = "EN_PROCESO",
                    Observaciones = $"Reconsulta programada para {proximaConsulta:dd/MM/yyyy HH:mm}",
                    FechaRespuesta = DateTime.UtcNow,
                    JsonCompleto = JsonSerializer.Serialize(request)
                };
            }

            

            var respuestaExistente = await _context.RespuestaCreditoFinanciera
                .FirstOrDefaultAsync(r => r.NumeroSolicitud == request.NumeroSolicitud.Trim());

            var respuesta = respuestaExistente ?? new RespuestaCreditoFinanciera
            {
                IdSolicitud = solicitud.Id,
                NumeroSolicitud = request.NumeroSolicitud.Trim()
            };

            respuesta.Estado = request.Estado.ToUpper().Trim();
            respuesta.MontoAprobado = request.MontoAprobado;
            respuesta.Tasa = request.Tasa;
            respuesta.Plazo = request.Plazo;
            respuesta.Observaciones = request.Observaciones?.Trim();
            if (request.Estado.ToUpper() == "CONDICIONADO" && request.CondicionesAprobacion != null)
            {
                var condicionesTexto = "Condiciones para aprobación: " +
                    string.Join(" | ", request.CondicionesAprobacion);

                respuesta.Observaciones = string.IsNullOrEmpty(respuesta.Observaciones)
                    ? condicionesTexto
                    : respuesta.Observaciones + " | " + condicionesTexto;
            }

            respuesta.FechaRespuesta = request.FechaRespuesta ?? DateTime.UtcNow;
            respuesta.JsonCompleto = JsonSerializer.Serialize(request);

            if (respuestaExistente == null)
                _context.RespuestaCreditoFinanciera.Add(respuesta);
            else
                _context.RespuestaCreditoFinanciera.Update(respuesta);

            // Generar notificación al asesor
            var mensajeBase = $"La financiera respondió {request.Estado.ToUpper()} para la solicitud {request.NumeroSolicitud}.";
            if (request.Estado.ToUpper() == "APROBADO" && request.MontoAprobado.HasValue)
                mensajeBase += $" Monto aprobado: ${request.MontoAprobado:N2}";

            var notificacion = new NotificacionAsesor
            {
                IdAsesor = solicitud.IdAsesor,
                IdSolicitud = solicitud.Id,
                Mensaje = mensajeBase
            };
            _context.NotificacionAsesor.Add(notificacion);

            await _context.SaveChangesAsync();

            // 2. Al final, cuando todo sale bien (cualquier estado)
            _logger.LogInformation("Respuesta de crédito procesada correctamente: {Solicitud} - {Estado}",
                request.NumeroSolicitud, request.Estado.ToUpper());

            return respuesta;
        }

        public async Task<RespuestaCreditoFinanciera?> ObtenerPorNumeroSolicitudAsync(string numeroSolicitud)
        {
            return await _context.RespuestaCreditoFinanciera
                .FirstOrDefaultAsync(r => r.NumeroSolicitud == numeroSolicitud.Trim());
        }
    }

}
