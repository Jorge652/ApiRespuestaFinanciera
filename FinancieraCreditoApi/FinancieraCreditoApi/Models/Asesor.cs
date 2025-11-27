using System.ComponentModel.DataAnnotations.Schema;

namespace FinancieraCreditoApi.Models
{
    [Table("Asesor")]
    public class Asesor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; } = true;
    }
}
