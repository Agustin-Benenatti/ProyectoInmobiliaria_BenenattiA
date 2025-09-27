using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliaria.Models
{
    public interface IRepositorioImagen : IRepositorio<ImagenModel>
    {
        IList<ImagenModel> BuscarPorInmueble(int IdInmueble);
    }
}