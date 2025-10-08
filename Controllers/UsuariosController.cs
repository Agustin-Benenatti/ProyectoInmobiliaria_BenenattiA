using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ProyectoInmobiliaria.Models;

namespace ProyectoInmobiliaria.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IRepositorioUsuario _repo;

        public UsuariosController(IRepositorioUsuario repo)
        {
            _repo = repo;
        }

        // GET: /Usuarios
        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View(_repo.ObtenerTodos());
        }

        // GET: /Usuarios/Login
        [AllowAnonymous]
        public IActionResult Login() => View();

        // POST: /Usuarios/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Debe ingresar email y contraseña.";
                return View();
            }

            var usuario = _repo.ObtenerPorEmail(email);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Email ?? ""),
                    new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario"),
                    new Claim("IdUsuario", usuario.IdUsuario.ToString()),
                    new Claim("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido}"),
                    new Claim("Avatar", string.IsNullOrEmpty(usuario.Avatar) ? "/images/default-avatar.png" : usuario.Avatar)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas.";
            return View();
        }

        // GET: /Usuarios/Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Cierra la sesión (invalida cookie de autenticación)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Elimina explícitamente la cookie de autenticación en el navegador
            Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToAction("Login");
        }

        // GET: /Usuarios/Crear
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear() => View();

        // POST: /Usuarios/Crear
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Usuario usuario, string password)
        {
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            if (string.IsNullOrEmpty(usuario.Avatar))
                usuario.Avatar = "/images/default-avatar.png";

            _repo.Alta(usuario);
            TempData["SuccessMessage"] = "Usuario creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Usuarios/EditarPerfil
        [Authorize]
        public IActionResult EditarPerfil()
        {
            var id = int.Parse(User.Claims.First(c => c.Type == "IdUsuario").Value);
            var usuario = _repo.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // POST: /Usuarios/EditarPerfil
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPerfil(Usuario model, string? nuevaClave, IFormFile? nuevoAvatar, bool eliminarAvatar = false)
        {
            var id = int.Parse(User.Claims.First(c => c.Type == "IdUsuario").Value);
            var usuario = _repo.ObtenerPorId(id);
            if (usuario == null) return NotFound();

            usuario.Nombre = model.Nombre;
            usuario.Apellido = model.Apellido;
            usuario.Email = model.Email;

            if (!string.IsNullOrEmpty(nuevaClave))
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevaClave);

            if (nuevoAvatar != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(nuevoAvatar.FileName)}";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "usuarios");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await nuevoAvatar.CopyToAsync(stream);
                }

                usuario.Avatar = "/images/usuarios/" + fileName;
            }
            else if (eliminarAvatar)
            {
                usuario.Avatar = "/images/default-avatar.png";
            }

            _repo.Modificacion(usuario);

            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email ?? ""),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario"),
                new Claim("IdUsuario", usuario.IdUsuario.ToString()),
                new Claim("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim("Avatar", string.IsNullOrEmpty(usuario.Avatar) ? "/images/default-avatar.png" : usuario.Avatar)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            TempData["Msg"] = "Perfil actualizado correctamente.";
            return RedirectToAction("EditarPerfil");
        }

        // GET: /Usuarios/Eliminar/5
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            var usuario = _repo.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // POST: /Usuarios/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            _repo.Baja(id);
            TempData["DeleteMessage"] = "Usuario eliminado correctamente."; 
            return RedirectToAction(nameof(Index));
        }
    }
}
