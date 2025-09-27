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

        // GET: Inmueble/Imagenes/5
        public IActionResult Imagenes(int id, [FromServices] IRepositorioImagen repoImagen)
        {
            var inmueble = _repo.ObtenerPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            // Traer las imágenes asociadas
            inmueble.Imagen = repoImagen.BuscarPorInmueble(id);

            // 👇 fuerza a usar la vista Imagen.cshtml
            return View("Imagen", inmueble);
        }

        // POST: Inmueble/SubirPortada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubirPortada(int inmuebleId, IFormFile archivo, [FromServices] IWebHostEnvironment environment)
        {
            if (archivo == null || archivo.Length == 0)
            {
                TempData["Error"] = "No se recibió ningún archivo.";
                return RedirectToAction(nameof(Detalle), new { id = inmuebleId });
            }

            try
            {
                var inmueble = _repo.ObtenerPorId(inmuebleId);
                if (inmueble == null) return NotFound();

                // Eliminar portada anterior si existe
                if (!string.IsNullOrEmpty(inmueble.PortadaUrl))
                {
                    var rutaAnterior = Path.Combine(environment.WebRootPath, inmueble.PortadaUrl.TrimStart('/'));
                    if (System.IO.File.Exists(rutaAnterior))
                    {
                        System.IO.File.Delete(rutaAnterior);
                    }
                }

                // Guardar nueva portada
                string wwwPath = environment.WebRootPath;
                string carpeta = Path.Combine(wwwPath, "Uploads", "Inmuebles");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                string nombreArchivo = "portada_" + inmuebleId + Path.GetExtension(archivo.FileName);
                string rutaFisica = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                }

                inmueble.PortadaUrl = $"/Uploads/Inmuebles/{nombreArchivo}";
                _repo.Modificacion(inmueble);

                TempData["Mensaje"] = "Portada actualizada correctamente";
                return RedirectToAction(nameof(Detalle), new { id = inmuebleId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Detalle), new { id = inmuebleId });
            }
        }

        // POST: Inmueble/EliminarPortada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarPortada(int inmuebleId, [FromServices] IWebHostEnvironment environment)
        {
            var inmueble = _repo.ObtenerPorId(inmuebleId);
            if (inmueble == null || string.IsNullOrEmpty(inmueble.PortadaUrl))
            {
                TempData["Error"] = "No hay portada para eliminar.";
                return RedirectToAction(nameof(Detalle), new { id = inmuebleId });
            }

            try
            {
                var rutaFisica = Path.Combine(environment.WebRootPath, inmueble.PortadaUrl.TrimStart('/'));
                if (System.IO.File.Exists(rutaFisica))
                {
                    System.IO.File.Delete(rutaFisica);
                }

                inmueble.PortadaUrl = null;
                _repo.Modificacion(inmueble);

                TempData["Mensaje"] = "Portada eliminada correctamente";
                return RedirectToAction(nameof(Detalle), new { id = inmuebleId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Detalle), new { id = inmuebleId });
            }
        }
    }
}
