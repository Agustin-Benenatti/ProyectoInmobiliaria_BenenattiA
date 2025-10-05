using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliaria.Models
{
    public class AuditoriaModel
    {
        public int IdAuditoria { get; set; }

        public string Entidad { get; set; } = string.Empty;
      
        public int EntidadId { get; set; }

        public int UsuarioId { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public string Accion { get; set; } = string.Empty;

        public string? Datos { get; set; }

        public string? Detalle { get; set; }
    }
}