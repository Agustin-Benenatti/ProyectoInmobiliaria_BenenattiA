using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoInmobiliaria.Models
{
    public class Contrato
    {
        public int IdContrato { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.Date)]
        public DateOnly FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.Date)]
        public DateOnly FechaFin { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(1, 100000000, ErrorMessage = "El precio debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no puede superar los 20 caracteres")]
        [RegularExpression("^(Activo|Inactivo)$", ErrorMessage = "El estado debe ser Activo o Inactivo")]
        public string? Estado { get; set; } = "Activo";

        [Required(ErrorMessage = "Debe seleccionar un inquilino")]
        [ForeignKey("Inquilino")]
        public int InquilinoId { get; set; }
        public Inquilino? Inquilino { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un inmueble")]
        [ForeignKey("Inmueble")]
        public int IdInmueble { get; set; }
        public Inmueble? Inmueble { get; set; }

        public decimal? Multa { get; set; }
        public DateOnly? FechaAnticipada { get; set; }
    }
}
