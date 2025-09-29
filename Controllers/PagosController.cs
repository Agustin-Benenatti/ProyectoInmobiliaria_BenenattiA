using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Models;

namespace ProyectoInmobiliaria.Controllers
{
    public class PagosController : Controller
    {
        private readonly IRepositorioPagos _repoPagos;
        private readonly IRepositorioContrato _repoContratos;
        private readonly IRepositorioPropietario _repoPropietarios;
        private readonly IRepositorioInquilino _repoInquilinos;

        public PagosController(
            IRepositorioPagos repoPagos,
            IRepositorioContrato repoContratos,
            IRepositorioPropietario repoPropietarios,
            IRepositorioInquilino repoInquilinos)
        {
            _repoPagos = repoPagos;
            _repoContratos = repoContratos;
            _repoPropietarios = repoPropietarios;
            _repoInquilinos = repoInquilinos;
        }

        // GET: Pagos
        public IActionResult Index(int pagina = 1)
        {
            int tamPag = 5;
            var lista = pagina <= 0
                ? _repoPagos.ObtenerTodos()
                : _repoPagos.ObtenerLista(pagina, tamPag);

            int total = _repoPagos.ObtenerCantidad();
            ViewBag.Pagina = pagina <= 0 ? 0 : pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamPag);

            return View(lista);
        }

        // GET: Pagos/Detalle/5
        public IActionResult Detalle(int id)
        {
            var pago = _repoPagos.ObtenerPorId(id);
            if (pago == null)
                return NotFound("No se encontró el Pago.");

            var contrato = _repoContratos.ObtenerPorId(pago.IdContrato);
            if (contrato == null)
                return NotFound("No se encontró el Contrato asociado.");

            // Inquilino
            var inquilino = contrato.Inquilino;
            ViewBag.InquilinoNombre = inquilino != null ? $"{inquilino.Nombre} {inquilino.Apellido}" : "No definido";

            // Propietario desde inmueble
            var inmueble = contrato.Inmueble;
            ViewBag.PropietarioNombre = inmueble?.Propietario != null
                ? $"{inmueble.Propietario.Nombre} {inmueble.Propietario.Apellido}"
                : "No definido";

            // Estado del contrato
            string estadoContrato;
            bool contratoTerminadoAnticipado = false;
            if (contrato.FechaAnticipada != null && contrato.FechaAnticipada < contrato.FechaFin)
            {
                estadoContrato = $"Finalizado anticipadamente el {contrato.FechaAnticipada:dd/MM/yyyy}";
                contratoTerminadoAnticipado = true;
            }
            else if (contrato.Estado == "Inactivo")
            {
                estadoContrato = "Finalizado";
            }
            else
            {
                estadoContrato = "Activo";
            }

            ViewBag.EstadoContrato = estadoContrato;
            ViewBag.Contrato = contrato;
            ViewBag.ContratoTerminadoAnticipado = contratoTerminadoAnticipado;

            return View(pago);
        }

        // GET: Pagos/Crear
        [HttpGet]
        public IActionResult Crear(int contratoId)
        {
            var contrato = _repoContratos.ObtenerPorId(contratoId);
            if (contrato == null)
                return NotFound("Contrato no encontrado.");

            var nroPago = _repoPagos.ObtenerTodos()
                             .Count(p => p.IdContrato == contratoId) + 1;

            var pago = new PagosModels
            {
                IdContrato = contrato.IdContrato,
                Monto = contrato.Precio,
                NroPago = nroPago,
                FechaPago = DateOnly.FromDateTime(DateTime.Today)
            };

            ViewBag.Contrato = contrato;
            return View(pago);
        }

        // POST: Pagos/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(PagosModels pago)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Contrato = _repoContratos.ObtenerPorId(pago.IdContrato);
                return View(pago);
            }

            _repoPagos.Alta(pago);
            return RedirectToAction(nameof(Index));
        }

        // GET: Pagos/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var pago = _repoPagos.ObtenerPorId(id);
            if (pago == null) return NotFound();

            ViewBag.Contrato = _repoContratos.ObtenerPorId(pago.IdContrato);
            return View(pago);
        }

        // POST: Pagos/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, PagosModels pago)
        {
            if (id != pago.IdPago) return BadRequest();
            if (!ModelState.IsValid) return View(pago);

            _repoPagos.Modificacion(pago);
            return RedirectToAction(nameof(Index));
        }

        // GET: Pagos/Anular/5
        [HttpGet]
        public IActionResult Anular(int id)
        {
            var pago = _repoPagos.ObtenerPorId(id);
            if (pago == null) return NotFound();

            ViewBag.Contrato = _repoContratos.ObtenerPorId(pago.IdContrato);
            return View(pago);
        }

        // POST: Pagos/AnularConfirmado
        [HttpPost, ActionName("AnularConfirmado")]
        [ValidateAntiForgeryToken]
        public IActionResult AnularConfirmado(int id)
        {
            var pago = _repoPagos.ObtenerPorId(id);
            if (pago == null) return NotFound();

            _repoPagos.AnularPago(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
