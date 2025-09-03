using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliaria.Models;

namespace ProyectoInmobiliaria.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IRepositorioContrato _repo;
        private readonly IRepositorioInquilino _repoInquilino;
        private readonly IRepositorioInmueble _repoInmueble;


        public ContratoController(IRepositorioContrato repo, IRepositorioInquilino repoInquilino, IRepositorioInmueble repoInmueble)
        {
            _repo = repo;
            _repoInquilino = repoInquilino;
            _repoInmueble = repoInmueble;
        }

        //GET: Contrato
        public IActionResult Index()
        {
            var lista = _repo.ObtenerTodos();
            return View(lista);
        }
        
        //// GET: Contrato/Detalle
        public IActionResult Detalle(int id)
        {
            var contrato = _repo.ObtenerPorId(id);

            if (contrato == null)
            {
                return NotFound();
            }

            return View(contrato);
        }

        // GET: Contrato/Crear
        public IActionResult Crear()
        {
            var inquilinos = _repoInquilino.ObtenerTodos();
            var inmuebles = _repoInmueble.ObtenerTodos();

            ViewBag.Inquilinos = new SelectList(
                inquilinos.Select(i => new { i.InquilinoId, NombreCompleto = i.Nombre + " " + i.Apellido }),
                "InquilinoId",
                "NombreCompleto"
            );
            ViewBag.Inmuebles = new SelectList(inmuebles, "IdInmueble", "Direccion");

            return View();
        }

        // POST: Contrato/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                _repo.Alta(contrato);
                return RedirectToAction(nameof(Index));
            }

            var inquilinos = _repoInquilino.ObtenerTodos();
            var inmuebles = _repoInmueble.ObtenerTodos();

            ViewBag.Inquilinos = new SelectList(
                inquilinos.Select(i => new { i.InquilinoId, NombreCompleto = i.Nombre + " " + i.Apellido }),
                "InquilinoId",
                "NombreCompleto",
                contrato.InquilinoId
            );
            ViewBag.Inmuebles = new SelectList(inmuebles, "IdInmueble", "Direccion", contrato.IdInmueble);

            return View(contrato);
        }

       // GET: Contrato/Editar
        public IActionResult Editar(int id)
        {
            var contrato = _repo.ObtenerPorId(id);
            if (contrato == null)
                return NotFound();

            // Obtener inquilinos y proyectar Nombre + Apellido
            var inquilinos = _repoInquilino.ObtenerTodos()
                .Select(i => new
                {
                    i.InquilinoId,
                    NombreCompleto = i.Nombre + " " + i.Apellido
                }).ToList();

            var inmuebles = _repoInmueble.ObtenerTodos();

            
            ViewBag.Inquilinos = new SelectList(inquilinos, "InquilinoId", "NombreCompleto", contrato.InquilinoId);
            ViewBag.Inmuebles = new SelectList(inmuebles, "IdInmueble", "Direccion", contrato.IdInmueble);

            return View(contrato);
        }

        // POST: Contrato/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, Contrato contrato)
        {
            if (id != contrato.IdContrato)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _repo.Modificacion(contrato);
                return RedirectToAction(nameof(Index));
            }

            
            var inquilinos = _repoInquilino.ObtenerTodos()
                .Select(i => new
                {
                    i.InquilinoId,
                    NombreCompleto = i.Nombre + " " + i.Apellido
                }).ToList();

            var inmuebles = _repoInmueble.ObtenerTodos();

            ViewBag.Inquilinos = new SelectList(inquilinos, "InquilinoId", "NombreCompleto", contrato.InquilinoId);
            ViewBag.Inmuebles = new SelectList(inmuebles, "IdInmueble", "Direccion", contrato.IdInmueble);

            return View(contrato);
        }

        // GET: Contrato/Eliminar
        public IActionResult Eliminar(int id)
        {
            var contrato = _repo.ObtenerPorId(id);
            if (contrato == null)
                return NotFound();

            return View(contrato);
        }

        // POST: Contrato/EliminarConfirmado
        [HttpPost, ActionName("EliminarConfirmado")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            _repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Contrato/Activos
        public IActionResult Activos()
        {
            var lista = _repo.BuscarContratosActivos();
            return View("Index", lista);
        }

        // GET: Contrato/PorInquilino
        public IActionResult PorInquilino(int inquilinoId)
        {
            var lista = _repo.BuscarPorInquilino(inquilinoId);
            return View("Index", lista);
        }

        // GET: Contrato/PorPropietario
        public IActionResult PorPropietario(int propietarioId)
        {
            var lista = _repo.BuscarPorPropietario(propietarioId);
            return View("Index", lista);
        }


    }
}