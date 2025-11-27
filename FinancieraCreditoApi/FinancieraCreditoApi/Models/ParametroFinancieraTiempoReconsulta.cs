using System.ComponentModel.DataAnnotations.Schema;

namespace FinancieraCreditoApi.Models
{
    [Table("ParametroFinancieraTiempoReconsulta")]
    public class ParametroFinancieraTiempoReconsulta
    {
        public int Id { get; set; }
        public int IdFinanciera { get; set; }
        public int MinutosReconsulta { get; set; }
        public bool Activo { get; set; } = true;

    } 
}
