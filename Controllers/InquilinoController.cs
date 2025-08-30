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
        private readonly IRepositorioInquilino _repo;

        
        public InquilinoController(IRepositorioInquilino repo)
        {
            _repo = repo;
        }

        // GET: Inquilino
        public IActionResult Index()
        {
            var lista = _repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Inquilino/Detalle/5
        public IActionResult Detalle(int id)
        {
            var inquilino = _repo.ObtenerPorId(id);

            if (inquilino == null)
                return NotFound("No se encontró el Inquilino.");

            return View(inquilino);
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

            _repo.Alta(i);

            return RedirectToAction("Index");
        }

        // GET: Inquilino/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var inquilino = _repo.ObtenerPorId(id);

            if (inquilino == null)
                return NotFound();

            return View(inquilino);
        }

        // POST: Inquilino/Editar
        [HttpPost]
        public IActionResult Editar(Inquilino i)
        {
            if (!ModelState.IsValid)
                return View(i);

            _repo.Modificacion(i);

            return RedirectToAction("Index");
        }

        // GET: Inquilino/Eliminar/5
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var inquilino = _repo.ObtenerPorId(id);

            if (inquilino == null)
                return NotFound();

            return View(inquilino);
        }

        // POST: Inquilino/EliminarConfirmado
        [HttpPost, ActionName("EliminarConfirmado")]
        public IActionResult EliminarConfirmado(int id)
        {
            _repo.Baja(id);

            return RedirectToAction("Index");
        }
    }
}