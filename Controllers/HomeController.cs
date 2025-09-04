using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Models;

namespace ProyectoInmobiliaria.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Route("Home/Error/{code?}")]
    public IActionResult Error(int? code = null)
    {
        var model = new ErrorViewModel();

        if (code == 404)
        {
            model.RequestId = "404";
            ViewBag.Mensaje = "La página que buscas no existe.";
        }
        else if (code == 500)
        {
            model.RequestId = "500";
            ViewBag.Mensaje = "Ocurrió un error en el servidor.";
        }
        else
        {
            model.RequestId = code?.ToString() ?? "Error";
            ViewBag.Mensaje = "Ocurrió un error inesperado.";
        }

        return View(model);
    }
}
