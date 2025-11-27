using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancieraCreditoApi.Models
{
    [Table("RespuestaCreditoFinanciera")]
    public class RespuestaCreditoFinanciera
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdSolicitud")]
        public int IdSolicitud { get; set; }

        [Column("NumeroSolicitud")]
        public string NumeroSolicitud { get; set; } = null!;

        [Column("Estado")]
        public string Estado { get; set; } = null!;

        [Column("MontoAprobado")]
        public decimal? MontoAprobado { get; set; }

        [Column("Tasa")]
        public decimal? Tasa { get; set; }

        [Column("Observaciones")]
        public string? Observaciones { get; set; }

        [Column("FechaRespuesta")]
        public DateTime FechaRespuesta { get; set; }

        [Column("JsonCompleto")]
        public string JsonCompleto { get; set; } = null!;

        [Column("FechaRegistro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Column("Plazo")]
        public int? Plazo { get; set; }  // número de meses

    }

}
