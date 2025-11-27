namespace FinancieraCreditoApi.Models
{
    public class SolicitudCredito
    {
        public int Id { get; set; }
        public string NumeroSolicitud { get; set; } = null!;
        public int IdAsesor { get; set; }
        public int IdFinanciera { get; set; }
        public DateTime FechaCreacion { get; set; }
       
    }
}
