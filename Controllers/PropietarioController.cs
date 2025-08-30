using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Models;

namespace ProyectoInmobiliaria.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly IRepositorioPropietario _repo;

        
        public PropietarioController(IRepositorioPropietario repo)
        {
            _repo = repo;
        }

        // GET: Propietario
        public IActionResult Index()
        {
            var lista = _repo.ObtenerTodos();
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
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var propietario = _repo.ObtenerPorId(id);
            if (propietario == null)
                return NotFound();

            return View(propietario);
        }

        // POST: Propietario/EliminarConfirmado
        [HttpPost, ActionName("EliminarConfirmado")]
        public IActionResult EliminarConfirmado(int id)
        {
            _repo.Baja(id);
            return RedirectToAction("Index");
        }
    }
}
