using System.ComponentModel.DataAnnotations;

namespace FinancieraCreditoApi.Models
{
    public class NotificacionAsesor
    {
        public int Id { get; set; }
        public int IdAsesor { get; set; } // FK con Asesor (si existe)
        public int IdSolicitud { get; set; }
        public string Mensaje { get; set; } = default!;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public bool Leido { get; set; } = false;
    }

}
