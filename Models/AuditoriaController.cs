using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Models;
using System;
using System.Linq;

namespace ProyectoInmobiliaria.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AuditoriaController : Controller
    {
        private readonly IRepositorioAuditoria _repo;

        public AuditoriaController(IRepositorioAuditoria repo)
        {
            _repo = repo;
        }

        public IActionResult Index(int page = 1, int pageSize = 5, string? entidad = null, int? entidadId = null)
        {
            try
            {
                var lista = Enumerable.Empty<AuditoriaModel>();
                int totalRegistros = 0;
                int totalPaginas = 0;

                if (string.IsNullOrEmpty(entidad) && !entidadId.HasValue)
                {
                    lista = _repo.ObtenerLista(page, pageSize);
                    totalRegistros = _repo.ObtenerCantidad();
                    totalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize);
                }
                else
                {
                    var auditoriasFiltradas = _repo.ObtenerTodos().AsQueryable();

                    if (!string.IsNullOrEmpty(entidad))
                        auditoriasFiltradas = auditoriasFiltradas
                            .Where(a => a.Entidad.Contains(entidad, StringComparison.OrdinalIgnoreCase));

                    if (entidadId.HasValue)
                        auditoriasFiltradas = auditoriasFiltradas
                            .Where(a => a.EntidadId == entidadId.Value);

                    totalRegistros = auditoriasFiltradas.Count();
                    totalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize);

                    lista = auditoriasFiltradas
                        .OrderByDescending(a => a.Fecha)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                }

                
                var idsUsuarios = lista.Select(a => a.UsuarioId).Distinct().ToList();
                var nombresUsuarios = _repo.ObtenerUsuariosPorIds(idsUsuarios);

                
                var vista = lista.Select(a => new
                {
                    a.IdAuditoria,
                    a.Entidad,
                    a.EntidadId,
                    a.Accion,
                    a.Fecha,
                    a.Detalle,
                    a.Datos,
                    UsuarioNombre = nombresUsuarios.ContainsKey(a.UsuarioId)
                        ? nombresUsuarios[a.UsuarioId]
                        : "Desconocido"
                }).ToList();

                ViewBag.Page = page;
                ViewBag.TotalPaginas = totalPaginas;
                ViewBag.TotalRegistros = totalRegistros;
                ViewBag.Entidad = entidad;
                ViewBag.EntidadId = entidadId;

                return View(vista);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar la auditoría: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }


        // GET: /Auditoria/Detalles/5
        public IActionResult Detalles(int id)
        {
            var auditoria = _repo.ObtenerPorId(id);
            if (auditoria == null)
                return NotFound();

            return View(auditoria);
        }

        [NonAction]
        public void RegistrarAccion(string entidad, int entidadId, string accion, int usuarioId, string? detalle = null, string? datos = null)
        {
            try
            {
                var nuevaAuditoria = new AuditoriaModel
                {
                    Entidad = entidad,
                    EntidadId = entidadId,
                    UsuarioId = usuarioId,
                    Accion = accion,
                    Fecha = DateTime.Now,
                    Detalle = detalle,
                    Datos = datos
                };
                _repo.Alta(nuevaAuditoria);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registrando auditoría: {ex.Message}");
            }
        }
    }
}
