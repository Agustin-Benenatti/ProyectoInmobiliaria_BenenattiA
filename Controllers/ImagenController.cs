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
    public class ImagenController : Controller
    {
        private readonly IRepositorioImagen _repo;

        public ImagenController(IRepositorioImagen repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Alta(int id, List<IFormFile> imagenes, [FromServices] IWebHostEnvironment environment)
        {
            if (imagenes == null || imagenes.Count == 0)
                return BadRequest("No se recibieron archivos.");

            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads", "Inmuebles", id.ToString());

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var file in imagenes)
            {
                if (file.Length > 0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                    var rutaArchivo = Path.Combine(path, nombreArchivo);

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var imagen = new ImagenModel
                    {
                        IdInmueble = id,
                        Url = $"/Uploads/Inmuebles/{id}/{nombreArchivo}"
                    };

                    _repo.Alta(imagen);
                }
            }

            return Ok(_repo.BuscarPorInmueble(id));
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            try
            {
                var entidad = _repo.ObtenerPorId(id);
                if (entidad == null || string.IsNullOrEmpty(entidad.Url))
                {
                    return NotFound($"No se encontro la imagen con ID");
                }

                // Borrar archivo fisico
                var rutaFisica = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    entidad.Url.TrimStart('/')
                );
                if (System.IO.File.Exists(rutaFisica))
                {
                    System.IO.File.Delete(rutaFisica);
                }

                _repo.Baja(id);

                return Ok(_repo.BuscarPorInmueble(entidad.IdInmueble));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}