using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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
        private readonly IRepositorioPagos _repoPagos;

        public ContratoController(IRepositorioContrato repo,
                                  IRepositorioInquilino repoInquilino,
                                  IRepositorioInmueble repoInmueble,
                                  IRepositorioPagos repoPagos)
        {
            _repo = repo;
            _repoInquilino = repoInquilino;
            _repoInmueble = repoInmueble;
            _repoPagos = repoPagos;
        }

        // GET: Contrato
        public IActionResult Index()
        {
            var lista = _repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Contrato/Detalle
        public IActionResult Detalle(int id)
        {
            var contrato = _repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();
            return View(contrato);
        }

        // GET: Contrato/Crear
        public IActionResult Crear()
        {
            ViewBag.Inquilinos = new SelectList(
                _repoInquilino.ObtenerTodos().Select(i => new { i.InquilinoId, NombreCompleto = i.Nombre + " " + i.Apellido }),
                "InquilinoId",
                "NombreCompleto"
            );
            ViewBag.Inmuebles = new SelectList(_repoInmueble.ObtenerTodos(), "IdInmueble", "Direccion");
            return View();
        }

        // POST: Contrato/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                if (contrato.FechaInicio != DateOnly.MinValue)
                    contrato.FechaFin = contrato.FechaInicio.AddMonths(6);

                _repo.Alta(contrato);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Inquilinos = new SelectList(
                _repoInquilino.ObtenerTodos().Select(i => new { i.InquilinoId, NombreCompleto = i.Nombre + " " + i.Apellido }),
                "InquilinoId",
                "NombreCompleto",
                contrato.InquilinoId
            );
            ViewBag.Inmuebles = new SelectList(_repoInmueble.ObtenerTodos(), "IdInmueble", "Direccion", contrato.IdInmueble);

            return View(contrato);
        }

        // GET: Contrato/Editar
        public IActionResult Editar(int id)
        {
            var contrato = _repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            ViewBag.Inquilinos = new SelectList(
                _repoInquilino.ObtenerTodos().Select(i => new { i.InquilinoId, NombreCompleto = i.Nombre + " " + i.Apellido }),
                "InquilinoId",
                "NombreCompleto",
                contrato.InquilinoId
            );
            ViewBag.Inmuebles = new SelectList(_repoInmueble.ObtenerTodos(), "IdInmueble", "Direccion", contrato.IdInmueble);

            return View(contrato);
        }

        // POST: Contrato/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, Contrato contrato)
        {
            if (id != contrato.IdContrato) return BadRequest();

            if (ModelState.IsValid)
            {
                if (contrato.FechaInicio != DateOnly.MinValue)
                    contrato.FechaFin = contrato.FechaInicio.AddMonths(6);

                _repo.Modificacion(contrato);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Inquilinos = new SelectList(
                _repoInquilino.ObtenerTodos().Select(i => new { i.InquilinoId, NombreCompleto = i.Nombre + " " + i.Apellido }),
                "InquilinoId",
                "NombreCompleto",
                contrato.InquilinoId
            );
            ViewBag.Inmuebles = new SelectList(_repoInmueble.ObtenerTodos(), "IdInmueble", "Direccion", contrato.IdInmueble);

            return View(contrato);
        }

        // GET: Contrato/Eliminar
        public IActionResult Eliminar(int id)
        {
            var contrato = _repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();
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

        // GET: Contrato/TerminarAnticipado
        public IActionResult TerminarAnticipado(int id)
        {
            var contrato = _repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();
            contrato.Multa ??= 0;
            return View(contrato);
        }

        // POST: Contrato/TerminarAnticipado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TerminarAnticipado(int id, DateOnly fechaAnticipada)
        {
            var contrato = _repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            // Calcular meses cumplidos
            var inicio = contrato.FechaInicio.ToDateTime(new TimeOnly(0, 0));
            var finAnticipado = fechaAnticipada.ToDateTime(new TimeOnly(0, 0));
            int totalMeses = ((finAnticipado.Year - inicio.Year) * 12) + (finAnticipado.Month - inicio.Month);
            if (finAnticipado.Day < inicio.Day) totalMeses--;

            // Calcular multa
            var multa = (totalMeses < 3) ? contrato.Precio * 2 : contrato.Precio;

            
            _repo.TerminarAnticipado(contrato.IdContrato, fechaAnticipada, multa);

            // Registrar pago de multa si aplica
            if (multa > 0)
            {
                var pagosContrato = _repoPagos.ObtenerPagosPorContrato(contrato.IdContrato);
                int nroPago = pagosContrato?.Any() == true ? pagosContrato.Max(p => p.NroPago) + 1 : 1;

                var pagoMulta = new PagosModels
                {
                    IdContrato = contrato.IdContrato,
                    NroPago = nroPago,
                    FechaPago = DateOnly.FromDateTime(DateTime.Now),
                    Monto = multa,
                    Detalle = $"Multa por terminación anticipada. Fecha de finalización: {fechaAnticipada:dd/MM/yyyy}",
                    Anulado = false
                };
                _repoPagos.Alta(pagoMulta);
            }

            TempData["InfoMulta"] = $"La multa calculada es {multa:C}";

            // Redirigir a Detalle; ya se mostrará Estado = 'Finalizado'
            return RedirectToAction(nameof(Detalle), new { id = contrato.IdContrato });
        }
    }
}
