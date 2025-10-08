using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Models;

namespace ProyectoInmobiliaria.Controllers
{
    [Authorize]
    public class PropietarioController : Controller
    {
        private readonly IRepositorioPropietario _repo;
        private readonly IRepositorioInmueble _repoInmueble;


        public PropietarioController(IRepositorioPropietario repo, IRepositorioInmueble repoInmueble)
        {
            _repo = repo;
            _repoInmueble = repoInmueble;
        }

        // GET: Propietario
        public IActionResult Index(int pagina = 1)
        {
            int tamPag = 5;
            IEnumerable<Propietario> lista;

            if (pagina <= 0)
            {
                // Sin paginación: trae todos los registros
                lista = _repo.ObtenerTodos();
                ViewBag.Pagina = 0;
                ViewBag.TotalPaginas = 0;
            }
            else
            {
                // Con paginación
                lista = _repo.ObtenerLista(pagina, tamPag);
                int total = _repo.ObtenerCantidad();
                ViewBag.Pagina = pagina;
                ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamPag);
            }

            return View(lista);
        }


        // GET: Propietario/Detalle/5
        public IActionResult Detalle(int id)
        {
            var propietario = _repo.ObtenerPorId(id);
            if (propietario == null)
                return NotFound("No se encontró el propietario.");

            return View(propietario);
        }

        // GET: Propietario/Crear
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Propietario/Crear
        [HttpPost]
        public IActionResult Crear(Propietario p)
        {

            if (!ModelState.IsValid)
                return View(p);

            _repo.Alta(p);
            TempData["SuccessMessage"] = "Propietario creado correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Propietario/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var propietario = _repo.ObtenerPorId(id);
            if (propietario == null)
                return NotFound();

            return View(propietario);
        }

        // POST: Propietario/Editar
        [HttpPost]
        public IActionResult Editar(Propietario p)
        {
            if (!ModelState.IsValid)
                return View(p);

            _repo.Modificacion(p);
            return RedirectToAction("Index");
        }

        // GET: Propietario/Eliminar/5
        [Authorize(Roles ="Administrador")]
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var propietario = _repo.ObtenerPorId(id);
            if (propietario == null)
                return NotFound();

            return View(propietario);
        }

        // POST: Propietario/EliminarConfirmado
        [Authorize(Roles ="Administrador")]
        [HttpPost, ActionName("EliminarConfirmado")]
        public IActionResult EliminarConfirmado(int id)
        {
             var inmuebles = _repoInmueble.BuscarPorPropietario(id);
            if (inmuebles.Any())
            {
                TempData["Error"] = "No se puede eliminar el propietario porque tiene inmuebles asociados.";
                return RedirectToAction(nameof(Index));
            }
            _repo.Baja(id);
            TempData["DeleteMessage"] = "Propietario eliminado correctamente.";
            return RedirectToAction("Index");
        }



    }
}
