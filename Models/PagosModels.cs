using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliaria.Models
{
    public class PagosModels
    {
        public int IdPago { get; set; }

        public DateOnly FechaPago { get; set; }

        public Decimal Monto { get; set; }

        public string? Detalle { get; set; }

        public int IdContrato { get; set; }
        
        public Contrato? Contrato { get; set;}
    }
}