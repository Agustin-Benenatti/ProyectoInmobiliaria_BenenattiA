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
    public class InquilinoController : Controller
    {
        private readonly IRepositorioInquilino _repo;
        private readonly IRepositorioContrato _repoContrato;


        public InquilinoController(IRepositorioInquilino repo, IRepositorioContrato repoContrato)
        {
            _repo = repo;
            _repoContrato = repoContrato;
        }

        // GET: Inquilino
        public IActionResult Index(int pagina = 1)
        {
            int tamPag = 3;
            IEnumerable<Inquilino> lista;

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
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Inquilino i)
        {
            if (!string.IsNullOrEmpty(i.Dni))
            {
                var inquilinoExistente = _repo.ObtenerPorDni(i.Dni);
                if (inquilinoExistente != null)
                {
                    ModelState.AddModelError("Dni", "Ya existe un inquilino registrado con ese DNI.");
                }
            }

            if (!string.IsNullOrEmpty(i.Email))
            {
                var inquilinoExistente = _repo.ObtenerPorEmail(i.Email);
                if (inquilinoExistente != null)
                {
                    ModelState.AddModelError("Email", "Ya existe un inquilino registrado con ese Email.");
                }
            }

            if (ModelState.IsValid)
            {
                _repo.Alta(i);
                TempData["SuccessMessage"] = "Inquilino creado correctamente.";
                return RedirectToAction("Index");
            }

            return View(i);
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
        [Authorize(Roles ="Administrador")]
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var inquilino = _repo.ObtenerPorId(id);

            if (inquilino == null)
                return NotFound();

            return View(inquilino);
        }

        // POST: Inquilino/EliminarConfirmado
        [Authorize(Roles ="Administrador")]
        [HttpPost, ActionName("EliminarConfirmado")]
        public IActionResult EliminarConfirmado(int id)
        {
            var contratos = _repoContrato.BuscarPorInquilino(id);
            if (contratos.Any())
            {
                TempData["Error"] = "No se puede eliminar el inquilino porque tiene contratos asociados.";
                return RedirectToAction(nameof(Index));
            }
            _repo.Baja(id);
            TempData["DeleteMessage"] = "Inquilino eliminado correctamente.";

            return RedirectToAction("Index");
        }


    }
}