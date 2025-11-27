using FinancieraCreditoApi.Models;
using FinancieraCreditoApi.DTOs;

namespace FinancieraCreditoApi.Services.Interfaces
{
    public interface IRespuestaCreditoService
    {
        Task<RespuestaCreditoFinanciera> ProcesarRespuestaAsync(RespuestaCreditoRequest request);
        Task<RespuestaCreditoFinanciera?> ObtenerPorNumeroSolicitudAsync(string numeroSolicitud);
    }
}
