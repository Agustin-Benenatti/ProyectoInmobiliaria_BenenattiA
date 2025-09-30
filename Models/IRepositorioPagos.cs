using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliaria.Models
{
    public interface IRepositorioPagos : IRepositorio<PagosModels>
    {
        IList<PagosModels> ObtenerPagosPorContrato(int IdContrato);
        IList<PagosModels> ObtenerLista(int pagNro, int tamPag);
        int ObtenerCantidad();
        void AnularPago(int idPago);
        int ObtenerUltimoNumeroPago(int idContrato);
    }
}