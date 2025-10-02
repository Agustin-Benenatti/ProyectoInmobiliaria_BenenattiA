using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliaria.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required]
        public  string? Nombre { get; set; }

        [Required]
        public string? Apellido { get; set; }

        [Required,EmailAddress]
        public string? Email { get; set; }
        
        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        public string? Rol { get; set; }

        public string? Avatar { get; set; }

        
    }
}