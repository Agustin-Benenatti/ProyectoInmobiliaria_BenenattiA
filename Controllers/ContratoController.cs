using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoInmobiliaria.Controllers
{
        [Authorize]
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
        public IActionResult Index(int pagina = 1)
        {
            int tamPag = 3;
            IEnumerable<Contrato> lista;

            if (pagina <= 0)
            {
                lista = _repo.ObtenerTodos();
                ViewBag.Pagina = 0;
                ViewBag.TotalPaginas = 0;
            }
            else
            {
                lista = _repo.ObtenerLista(pagina, tamPag);
                int total = _repo.ObtenerCantidad();
                ViewBag.Pagina = pagina;
                ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamPag);
            }


            var pagosPorContrato = lista.ToDictionary(
                c => c.IdContrato,
                c => _repoPagos.ObtenerPagosPorContrato(c.IdContrato)?.Count(p => !p.Anulado) ?? 0
            );
            ViewBag.PagosPorContrato = pagosPorContrato;


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
                _repoInquilino.ObtenerTodos()
                    .Select(i => new { i.InquilinoId, NombreCompleto = i.Nombre + " " + i.Apellido }),
                "InquilinoId",
                "NombreCompleto"
            );

            ViewBag.Inmuebles = new SelectList(
                _repoInmueble.ObtenerTodos(),
                "IdInmueble",
                "Direccion"
            );

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

                // Verificar solapamiento de fechas antes de guardar
                if (_repo.ExisteSolapamiento(contrato.IdInmueble, contrato.FechaInicio, contrato.FechaFin))
                {
                    ModelState.AddModelError("", "El inmueble ya tiene un contrato activo que se solapa con las fechas seleccionadas.");
                }
                else
                {
                    contrato.Estado = "Activo";
                    _repo.Alta(contrato);
                    return RedirectToAction(nameof(Index));
                }
            }

            // Si hubo error, recargar selects
            ViewBag.Inquilinos = new SelectList(
                _repoInquilino.ObtenerTodos()
                    .Select(i => new { i.InquilinoId, NombreCompleto = i.Nombre + " " + i.Apellido }),
                "InquilinoId",
                "NombreCompleto",
                contrato.InquilinoId
            );

            ViewBag.Inmuebles = new SelectList(
                _repoInmueble.ObtenerTodos(),
                "IdInmueble",
                "Direccion",
                contrato.IdInmueble
            );

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
        [Authorize(Roles ="Administrador")]
        public IActionResult Eliminar(int id)
        {
            var contrato = _repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();
            return View(contrato);
        }

        // POST: Contrato/EliminarConfirmado
        [HttpPost, ActionName("EliminarConfirmado")]
        [Authorize(Roles ="Administrador")]
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


            return RedirectToAction(nameof(Detalle), new { id = contrato.IdContrato });
        }

        private bool PuedeRenovarContrato(int contratoId)
        {
            var contrato = _repo.ObtenerPorId(contratoId);
            if (contrato == null) return false;

            // Solo se puede renovar si el contrato está finalizado
            if (contrato.Estado != "Finalizado")
                return false;

            // Verificar que tenga al menos 6 pagos hechos (no anulados)
            var pagos = _repoPagos.ObtenerPagosPorContrato(contratoId);
            if (pagos == null) return false;

            var pagosRealizados = pagos.Count(p => !p.Anulado);
            return pagosRealizados >= 6;
        }


        public IActionResult Renovar(int id)
        {
            if (!PuedeRenovarContrato(id))
            {
                TempData["Error"] = "El contrato debe estar Finalizado y tener al menos 6 pagos para poder renovarse.";
                return RedirectToAction(nameof(Index));
            }

            var contrato = _repo.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            var nuevoContrato = new Contrato
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.Today),
                FechaFin = DateOnly.FromDateTime(DateTime.Today.AddMonths(6)),
                Precio = contrato.Precio,
                InquilinoId = contrato.InquilinoId,
                IdInmueble = contrato.IdInmueble,
                Estado = "Activo",
                Inquilino = contrato.Inquilino,
                Inmueble = contrato.Inmueble
            };

            return View(nuevoContrato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Renovar(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                // Siempre 6 meses de duración
                contrato.FechaFin = contrato.FechaInicio.AddMonths(6);
                contrato.Estado = "Activo";

                var nuevoId = _repo.Alta(contrato);
                TempData["Info"] = $"Contrato renovado con Id={nuevoId}";

                return RedirectToAction(nameof(Index));
            }

            return View(contrato);
        }

    }
}
