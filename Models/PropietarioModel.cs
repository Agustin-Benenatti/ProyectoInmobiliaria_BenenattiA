using System.ComponentModel.DataAnnotations;

namespace ProyectoInmobiliaria.Models
{
    public class Propietario
    {
        
        public int PropietarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe tener entre 7 y 8 dígitos.")]
        public string? Dni { get; set; }

        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Formato de teléfono inválido.")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string? Email { get; set; }
    }
}
