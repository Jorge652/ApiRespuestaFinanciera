using System.ComponentModel.DataAnnotations.Schema;

namespace FinancieraCreditoApi.Models
{
    [Table("Financiera")]
    public class Financiera
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; } = true;
    }
}
