using FinancieraCreditoApi.DTOs;
using FinancieraCreditoApi.Models;
using FinancieraCreditoApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FinancieraCreditoApi.Controllers
{
    [ApiController]
    [Route("api/creditos")]
    public class CreditoRespuestaController : ControllerBase
    {
        private readonly IRespuestaCreditoService _service;

        public CreditoRespuestaController(IRespuestaCreditoService service)
        {
            _service = service;
        }

        [HttpPost("respuesta")]
        public async Task<IActionResult> RecibirRespuesta([FromBody] RespuestaCreditoRequest request)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(request.NumeroSolicitud))
                errores.Add("NumeroSolicitud es obligatorio");
            else if (request.NumeroSolicitud.Length > 50)
                errores.Add("NumeroSolicitud no puede exceder 50 caracteres");

            if (string.IsNullOrWhiteSpace(request.Estado))
                errores.Add("Estado es obligatorio");

            var estadosValidos = new[] { "APROBADO", "NEGADO", "CONDICIONADO", "REQUIERE DOCUMENTOS", "EN_PROCESO" };
            if (!estadosValidos.Contains(request.Estado.Trim().ToUpper()))
                errores.Add("Estado permitido: APROBADO, NEGADO, CONDICIONADO, REQUIERE DOCUMENTOS, EN_PROCESO");

            var estadoUpper = request.Estado.Trim().ToUpper();

            if (estadoUpper == "APROBADO")
            {
                if (!request.MontoAprobado.HasValue || request.MontoAprobado <= 0)
                    errores.Add("MontoAprobado es obligatorio y debe ser mayor a 0 cuando el estado es APROBADO");


                if (!request.Plazo.HasValue || request.Plazo <= 0)
                    errores.Add("Plazo (número de meses) es obligatorio y debe ser mayor a 0 cuando el estado es APROBADO");
            }

            if (estadoUpper == "NEGADO")
            {
                if (string.IsNullOrWhiteSpace(request.Observaciones))
                    errores.Add("Observaciones son obligatorias cuando el estado es NEGADO");
            }

            if (estadoUpper == "CONDICIONADO")
            {
                if (request.CondicionesAprobacion == null || !request.CondicionesAprobacion.Any())
                {
                    errores.Add("Cuando el estado es CONDICIONADO, debe enviar al menos una condición en 'condicionesAprobacion'");
                }
                else if (request.CondicionesAprobacion.Any(c => string.IsNullOrWhiteSpace(c)))
                {
                    errores.Add("Todas las condiciones deben tener texto");
                }
            }

            // para el error 400 BadRequest
            if (errores.Any())
                return BadRequest(new { mensaje = "Errores de validación", errores });


            try
            {
                var respuesta = await _service.ProcesarRespuestaAsync(request);
                return Ok(new
                {
                    mensaje = "Respuesta de crédito procesada correctamente",
                    data = respuesta
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("respuesta/{numeroSolicitud}")]
        public async Task<IActionResult> ObtenerRespuesta(string numeroSolicitud)
        {
            if (string.IsNullOrWhiteSpace(numeroSolicitud))
                return BadRequest("El número de solicitud es obligatorio");

            var respuesta = await _service.ObtenerPorNumeroSolicitudAsync(numeroSolicitud.Trim());
            if (respuesta == null)
                return NotFound($"No existe respuesta para la solicitud {numeroSolicitud}");

            return Ok(respuesta);
        }
    }
}
