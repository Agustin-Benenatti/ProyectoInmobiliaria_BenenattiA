using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliaria.Models
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        Usuario ObtenerPorEmail(string email);

        IList<Usuario> ObtenerLista(int paginaNro = 1, int tamPagina = 10);
        int ObtenerCantidad();

    }
}