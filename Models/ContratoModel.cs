using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoInmobiliaria.Models
{
    public class Contrato
    {
        public int IdContrato { get; set; }

        [Required]
        public DateOnly FechaInicio { get; set; }

        [Required]
        public DateOnly FechaFin { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public string? Estado { get; set; } = "Activo";

        [Required]
        [ForeignKey("Inquilino")]
        public int InquilinoId { get; set; }
        public Inquilino? Inquilino { get; set; }

        [Required]
        [ForeignKey("Inmueble")]
        public int IdInmueble { get; set; }
        public Inmueble? Inmueble { get; set; }

    }
}