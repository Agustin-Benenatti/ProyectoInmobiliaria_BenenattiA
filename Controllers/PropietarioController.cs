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
        private readonly IConfiguration _configuration;

        public PropietarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Propietario
        public IActionResult Index()
        {
            var repo = new RepositorioPropietario(_configuration);
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Propietario/Detalle/5
        public IActionResult Detalle(int id)
        {
            var repo = new RepositorioPropietario(_configuration);
            var propietario = repo.ObtenerPorId(id);

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

            var repo = new RepositorioPropietario(_configuration);
            repo.Alta(p);

            return RedirectToAction("Index");
        }

        // GET: Propietario/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var repo = new RepositorioPropietario(_configuration);
            var propietario = repo.ObtenerPorId(id);

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

            var repo = new RepositorioPropietario(_configuration);
            repo.Modificacion(p);

            return RedirectToAction("Index");
        }

        // GET: Propietario/Eliminar/5
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var repo = new RepositorioPropietario(_configuration);
            var propietario = repo.ObtenerPorId(id);

            if (propietario == null)
                return NotFound();

            return View(propietario);
        }

        // POST: Propietario/EliminarConfirmado
        [HttpPost, ActionName("EliminarConfirmado")]
        public IActionResult EliminarConfirmado(int id)
        {
            var repo = new RepositorioPropietario(_configuration);
            repo.Baja(id);

            return RedirectToAction("Index");
        }
    }
}
