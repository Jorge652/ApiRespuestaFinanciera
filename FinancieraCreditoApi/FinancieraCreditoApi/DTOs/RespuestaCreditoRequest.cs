namespace FinancieraCreditoApi.DTOs
{
    public class RespuestaCreditoRequest
    {
        public string NumeroSolicitud { get; set; } = default!; 
        public string Estado { get; set; } = default!; 
        public decimal? MontoAprobado { get; set; } 
        public decimal? Tasa { get; set; }
        public string? Observaciones { get; set; } 
        public DateTime? FechaRespuesta { get; set; }

        public int? Plazo { get; set; }  

        //Lista para validar lo de CONDICIONADO
        public List<string>? CondicionesAprobacion { get; set; }
    }

}
