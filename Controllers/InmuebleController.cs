using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliaria.Models;

namespace ProyectoInmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble _repo;
        private readonly IRepositorioPropietario _repoPropietario;

        public InmuebleController(IRepositorioInmueble repo, IRepositorioPropietario repoPropietario)
        {
            _repo = repo;
            _repoPropietario = repoPropietario;
        }

        // GET: Inmueble
        public IActionResult Index()
        {
            var lista = _repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Inmueble/Detalle
        public IActionResult Detalle(int id)
        {
            var inmueble = _repo.ObtenerPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            if (inmueble.PropietarioId.HasValue)
            {
                inmueble.Propietario = _repoPropietario.ObtenerPorId(inmueble.PropietarioId.Value);
            }
            return View(inmueble);
        }

        // GET: Inmueble/Crear
        public IActionResult Crear()
        {
            var propietarios = _repoPropietario.ObtenerTodos();
            ViewBag.Propietarios = new SelectList(propietarios, "PropietarioId", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                _repo.Alta(inmueble);
                return RedirectToAction(nameof(Index));
            }

            var propietarios = _repoPropietario.ObtenerTodos();
            ViewBag.Propietarios = new SelectList(propietarios, "PropietarioId", "Nombre", inmueble.PropietarioId);
            return View(inmueble);
        }

        // GET: Inmueble/Editar
        public IActionResult Editar(int id)
        {
            var inmueble = _repo.ObtenerPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            var propietarios = _repoPropietario.ObtenerTodos();
            ViewBag.Propietarios = new SelectList(propietarios, "PropietarioId", "Nombre", inmueble.PropietarioId);
            return View(inmueble);
        }

        // POST: Inmueble/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, Inmueble inmueble)
        {
            if (id != inmueble.IdInmueble)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _repo.Modificacion(inmueble);
                return RedirectToAction(nameof(Index));
            }

            var propietarios = _repoPropietario.ObtenerTodos();
            ViewBag.Propietarios = new SelectList(propietarios, "PropietarioId", "Nombre", inmueble.PropietarioId);
            return View(inmueble);
        }

        // GET: Inmueble/Eliminar
        public IActionResult Eliminar(int id)
        {
            var inmueble = _repo.ObtenerPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            if (inmueble.PropietarioId.HasValue)
            {
                inmueble.Propietario = _repoPropietario.ObtenerPorId(inmueble.PropietarioId.Value);
            }
            return View(inmueble);
        }

        // POST: Inmueble/EliminarConfirmado
        [HttpPost, ActionName("EliminarConfirmado")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            _repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Inmueble/PorPropietario
        public IActionResult PorPropietario(int id)
        {
            var lista = _repo.BuscarPorPropietario(id);
            return View("PropiedadesPorPropietario", lista);
        }
    }
}
