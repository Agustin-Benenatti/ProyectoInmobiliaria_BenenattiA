using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Models;

namespace ProyectoInmobiliaria.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly IConfiguration _configuration;

        public InquilinoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

         // GET: InquilinoRepositorioInquilino
        public IActionResult Index()
        {
            var repo = new RepositorioInquilino(_configuration);
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Inquilino/Detalle/5
        public IActionResult Detalle(int id)
        {
            var repo = new RepositorioInquilino(_configuration);
            var Inquilino = repo.ObtenerPorId(id);

            if (Inquilino == null)
                return NotFound("No se encontró el Inquilino.");

            return View(Inquilino);
        }

        // GET: Inquilino/Crear
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Inquilino/Crear
        [HttpPost]
        public IActionResult Crear(Inquilino i)
        {
            if (!ModelState.IsValid)
                return View(i);

            var repo = new RepositorioInquilino(_configuration);
            repo.Alta(i);

            return RedirectToAction("Index");
        }

        // GET: Inquilino/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var repo = new RepositorioInquilino(_configuration);
            var InquilinoRepositorioInquilino = repo.ObtenerPorId(id);

            if (InquilinoRepositorioInquilino == null)
                return NotFound();

            return View(InquilinoRepositorioInquilino);
        }

        // POST: Inquilino/Editar
        [HttpPost]
        public IActionResult Editar(Inquilino i)
        {
            if (!ModelState.IsValid)
                return View(i);

            var repo = new RepositorioInquilino(_configuration);
            repo.Modificacion(i);

            return RedirectToAction("Index");
        }

        // GET: Inquilino/Eliminar/5
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var repo = new RepositorioInquilino(_configuration);
            var InquilinoRepositorioInquilino = repo.ObtenerPorId(id);

            if (InquilinoRepositorioInquilino == null)
                return NotFound();

            return View(InquilinoRepositorioInquilino);
        }

        // POST: Inquilino/EliminarConfirmado
        [HttpPost, ActionName("EliminarConfirmado")]
        public IActionResult EliminarConfirmado(int id)
        {
            var repo = new RepositorioInquilino(_configuration);
            repo.Baja(id);

            return RedirectToAction("Index");
        }
    }
}