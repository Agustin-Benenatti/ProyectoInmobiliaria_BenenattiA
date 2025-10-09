using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliaria.Models
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        IList<Contrato> BuscarContratosActivos();
        IList<Contrato> BuscarPorInquilino(int InquilidoId);
        IList<Contrato> BuscarPorPropietario(int PrpietarioId);
        IList<Contrato> ObtenerLista(int paginaNro, int tamPag);
        public int TerminarAnticipado(int id, DateOnly fechaAnticipada, decimal multa);
        int ObtenerCantidad();
        int RenovarContrato(int idContrato);
        bool ExisteSolapamiento(int idInmueble, DateOnly inicio, DateOnly fin, int? idContrato = null);
        IList<Contrato> BuscarVigentesEntreFechas(DateOnly desde, DateOnly hasta);
        IList<Contrato> BuscarPorPlazo(int dias);
        IList<Contrato> ObtenerPorInmueble(int idInmueble);
    }
}