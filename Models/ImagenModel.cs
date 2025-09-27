using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliaria.Models
{
    public class ImagenModel
    {
        public int ImagenId { get; set; }

        public string? Url { get; set; }

        public int IdInmueble { get; set; }

        public Inmueble? Inmueble { get; set; }

        public IFormFile? Archivo { get; set; }


    }
}