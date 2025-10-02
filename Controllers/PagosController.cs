using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoInmobiliaria.Controllers
{
    [Authorize]
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
        public IActionResult Index(int contratoId = 0, int pagina = 1)
        {
            int tamPag = 5;
            IEnumerable<PagosModels> lista;

            if (contratoId > 0)
            {
                lista = _repoPagos.ObtenerPagosPorContrato(contratoId);
                ViewBag.Contrato = _repoContratos.ObtenerPorId(contratoId);
                ViewBag.MostrarFiltroContrato = true;
                ViewBag.TotalPagos = lista.Count();
            }
            else
            {
                lista = pagina <= 0
                    ? _repoPagos.ObtenerTodos()
                    : _repoPagos.ObtenerLista(pagina, tamPag);

                int total = _repoPagos.ObtenerCantidad();
                ViewBag.Pagina = pagina <= 0 ? 0 : pagina;
                ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamPag);
            }

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

            ViewBag.InquilinoNombre = contrato.Inquilino != null ?
                $"{contrato.Inquilino.Nombre} {contrato.Inquilino.Apellido}" : "No definido";

            ViewBag.PropietarioNombre = contrato.Inmueble?.Propietario != null ?
                $"{contrato.Inmueble.Propietario.Nombre} {contrato.Inmueble.Propietario.Apellido}" : "No definido";

            string estadoContrato;
            bool contratoTerminadoAnticipado = false;
            if (contrato.FechaAnticipada != null && contrato.FechaAnticipada < contrato.FechaFin)
            {
                estadoContrato = $"Finalizado anticipadamente el {contrato.FechaAnticipada:dd/MM/yyyy}";
                contratoTerminadoAnticipado = true;
            }
            else if (contrato.Estado == "Inactivo" || contrato.Estado == "Finalizado")
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

            var pagosExistentes = _repoPagos.ObtenerPagosPorContrato(contratoId)?.Count(p => !p.Anulado) ?? 0;

            bool contratoFinalizado = contrato.Estado == "Inactivo" ||
                                    contrato.Estado == "Finalizado" ||
                                    (contrato.FechaAnticipada != null && contrato.FechaAnticipada < contrato.FechaFin) ||
                                    pagosExistentes >= 6;

            var pago = new PagosModels
            {
                IdContrato = contrato.IdContrato,
                Monto = contrato.Precio,
                NroPago = pagosExistentes + 1,
                FechaPago = DateOnly.FromDateTime(DateTime.Today)
            };

            ViewBag.Contrato = contrato;
            ViewBag.ContratoFinalizado = contratoFinalizado;
            return View(pago);
        }

        // POST: Pagos/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(PagosModels pago)
        {
            var contrato = _repoContratos.ObtenerPorId(pago.IdContrato);
            if (contrato == null)
                return NotFound("Contrato no encontrado.");

            var pagosExistentes = _repoPagos.ObtenerPagosPorContrato(pago.IdContrato)?.Count(p => !p.Anulado) ?? 0;

            bool contratoFinalizado = contrato.Estado == "Inactivo" ||
                                      contrato.Estado == "Finalizado" ||
                                      (contrato.FechaAnticipada != null && contrato.FechaAnticipada < contrato.FechaFin) ||
                                      pagosExistentes >= 6;

            if (contratoFinalizado)
            {
                TempData["Error"] = "No se pueden registrar pagos porque el contrato ya está finalizado.";
                return RedirectToAction("Index", new { contratoId = pago.IdContrato });
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Contrato = contrato;
                return View(pago);
            }

            _repoPagos.Alta(pago);
            pagosExistentes++;

            if (pagosExistentes >= 6)
            {

                _repoContratos.TerminarAnticipado(contrato.IdContrato, contrato.FechaFin, 0);
                TempData["Mensaje"] = "Pago registrado correctamente. Se completaron los 6 pagos y el contrato ha sido finalizado.";
            }
            else
            {
                TempData["Mensaje"] = "Pago registrado correctamente.";
            }

            return RedirectToAction(nameof(Index), new { contratoId = pago.IdContrato });
        }

        // GET: Pagos/FinalizarAnticipado
        [HttpGet]
        public IActionResult FinalizarAnticipado(int contratoId)
        {
            var contrato = _repoContratos.ObtenerPorId(contratoId);
            if (contrato == null)
                return NotFound("Contrato no encontrado.");

            var pagosRegistrados = _repoPagos.ObtenerPagosPorContrato(contratoId)?.Count(p => !p.Anulado) ?? 0;
            if (pagosRegistrados < 1)
            {
                TempData["Error"] = "No se puede finalizar anticipadamente: debes tener al menos un pago registrado.";
                return RedirectToAction("Index", new { contratoId });
            }

            return View(contrato);
        }

        // POST: Pagos/FinalizarAnticipado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizarAnticipado(int contratoId, DateTime fechaAnticipada)
        {
            var contrato = _repoContratos.ObtenerPorId(contratoId);
            if (contrato == null) return NotFound("Contrato no encontrado.");

            var pagosRegistrados = _repoPagos.ObtenerPagosPorContrato(contratoId)?.Count(p => !p.Anulado) ?? 0;
            if (pagosRegistrados < 1)
            {
                TempData["Error"] = "No se puede finalizar anticipadamente sin haber registrado al menos un pago.";
                return RedirectToAction("Index", new { contratoId });
            }

            DateOnly fechaAnticipadaDateOnly = DateOnly.FromDateTime(fechaAnticipada);

            if (fechaAnticipadaDateOnly < contrato.FechaInicio)
            {
                TempData["Error"] = "La fecha de finalización anticipada no puede ser anterior a la fecha de inicio del contrato.";
                return RedirectToAction("Index", new { contratoId });
            }

            double totalDias = (contrato.FechaFin.ToDateTime(TimeOnly.MinValue) - contrato.FechaInicio.ToDateTime(TimeOnly.MinValue)).TotalDays;
            double diasCumplidos = (fechaAnticipadaDateOnly.ToDateTime(TimeOnly.MinValue) - contrato.FechaInicio.ToDateTime(TimeOnly.MinValue)).TotalDays;

            decimal multa = diasCumplidos < totalDias / 2 ? contrato.Precio * 2 : contrato.Precio;


            _repoContratos.TerminarAnticipado(contrato.IdContrato, fechaAnticipadaDateOnly, multa);

            TempData["Mensaje"] = $"Contrato finalizado anticipadamente. Multa aplicada: {multa:C}";
            return RedirectToAction("Index", new { contratoId });
        }

        // GET: Pagos/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var pago = _repoPagos.ObtenerPorId(id);
            if (pago == null) return NotFound();

            var contrato = _repoContratos.ObtenerPorId(pago.IdContrato);
            bool contratoFinalizado = contrato.Estado == "Inactivo" ||
                                      contrato.Estado == "Finalizado" ||
                                      (contrato.FechaAnticipada != null && contrato.FechaAnticipada < contrato.FechaFin);

            if (contratoFinalizado)
            {
                TempData["Error"] = "No se puede editar pagos de contratos finalizados.";
                return RedirectToAction("Index", new { contratoId = contrato.IdContrato });
            }

            ViewBag.Contrato = contrato;
            return View(pago);
        }

        // POST: Pagos/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, PagosModels pago)
        {
            var contrato = _repoContratos.ObtenerPorId(pago.IdContrato);
            bool contratoFinalizado = contrato.Estado == "Inactivo" ||
                                      contrato.Estado == "Finalizado" ||
                                      (contrato.FechaAnticipada != null && contrato.FechaAnticipada < contrato.FechaFin);

            if (contratoFinalizado)
            {
                TempData["Error"] = "No se puede editar pagos de contratos finalizados.";
                return RedirectToAction("Index", new { contratoId = contrato.IdContrato });
            }

            if (id != pago.IdPago) return BadRequest();
            if (!ModelState.IsValid) return View(pago);

            _repoPagos.Modificacion(pago);
            TempData["Mensaje"] = "Pago editado correctamente.";
            return RedirectToAction(nameof(Index), new { contratoId = pago.IdContrato });
        }

        // GET: Pagos/Anular/5
        [HttpGet]
        public IActionResult Anular(int id)
        {
            var pago = _repoPagos.ObtenerPorId(id);
            if (pago == null) return NotFound();

            var contrato = _repoContratos.ObtenerPorId(pago.IdContrato);
            bool contratoFinalizado = contrato.Estado == "Inactivo" ||
                                      contrato.Estado == "Finalizado" ||
                                      (contrato.FechaAnticipada != null && contrato.FechaAnticipada < contrato.FechaFin);

            if (contratoFinalizado)
            {
                TempData["Error"] = "No se puede anular pagos de contratos finalizados.";
                return RedirectToAction("Index", new { contratoId = contrato.IdContrato });
            }

            ViewBag.Contrato = contrato;
            return View(pago);
        }

        // POST: Pagos/AnularConfirmado
        [HttpPost, ActionName("AnularConfirmado")]
        [ValidateAntiForgeryToken]
        public IActionResult AnularConfirmado(int id)
        {
            if (id <= 0) return NotFound("Pago no válido.");

            var pago = _repoPagos.ObtenerPorId(id);
            if (pago == null) return NotFound("No se encontró el pago.");

            _repoPagos.AnularPago(id);

            var contratoOriginal = _repoContratos.ObtenerPorId(pago.IdContrato);
            if (contratoOriginal != null)
            {
                var pagosActivos = _repoPagos.ObtenerPagosPorContrato(contratoOriginal.IdContrato)
                                   ?.Count(p => !p.Anulado) ?? 0;

                if (pagosActivos < 6 && contratoOriginal.Estado == "Finalizado")
                {
                    contratoOriginal.Estado = "Activo";
                    _repoContratos.Modificacion(contratoOriginal);
                }
            }

            TempData["Mensaje"] = "Pago anulado correctamente.";
            return RedirectToAction(nameof(Index), new { contratoId = pago.IdContrato });
        }
    }
}
