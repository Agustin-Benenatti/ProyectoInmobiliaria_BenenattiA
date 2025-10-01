using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliaria.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> BuscarPorPropietario(int idPropietario);
        bool InmuebleDisponible(int idInmueble);
        IList<Inmueble> ObtenerDisponibles();
    }
}