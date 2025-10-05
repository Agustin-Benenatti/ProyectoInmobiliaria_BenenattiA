using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliaria.Models
{
    public interface IRepositorioAuditoria
    {
        int Alta(AuditoriaModel auditoria);
        List<AuditoriaModel> ObtenerPorEntidad(string entidad, int entidadId);

        List<AuditoriaModel> ObtenerTodos();
        AuditoriaModel? ObtenerPorId(int id);
        List<AuditoriaModel> ObtenerLista(int paginaNro, int tamPag);
        int ObtenerCantidad();
        Dictionary<int, string> ObtenerUsuariosPorIds(List<int> ids);
    }
}