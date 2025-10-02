using ProyectoInmobiliaria.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//inyeccion de dependencia 
builder.Services.AddScoped<IRepositorioPropietario, RepositorioPropietario>();
builder.Services.AddScoped<IRepositorioInquilino, RepositorioInquilino>();
builder.Services.AddScoped<IRepositorioInmueble, RepositorioInmueble>();
builder.Services.AddScoped<IRepositorioContrato, RepositorioContrato>();
builder.Services.AddScoped<IRepositorioImagen, RepositorioImagen>();
builder.Services.AddScoped<IRepositorioPagos, RepositorioPagos>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();

// Configuración de autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";            // si no está logueado, va acá
        options.LogoutPath = "/Usuarios/Logout";          // ruta para cerrar sesión
        options.AccessDeniedPath = "/Home/AccesoDenegado";   // si no tiene permisos
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
app.UseExceptionHandler("/Home/Error/500");

app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();

//Orden importante: primero autenticacion, después autorizacion
app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();
// Crear usuario administrador si no existe
using (var scope = app.Services.CreateScope())
{
    var servicios = scope.ServiceProvider;
    var repoUsuarios = servicios.GetRequiredService<IRepositorioUsuario>();

    var admin = repoUsuarios.ObtenerPorEmail("admin@admin.com");
    if (admin == null)
    {
        var usuarioAdmin = new Usuario
        {
            Nombre = "Admin",
            Apellido = "Principal",
            Email = "admin@admin.com",
            Rol = "Administrador",
            Avatar = "/img/default-avatar.png",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123") // ⚠️ Cambiar luego
        };
        repoUsuarios.Alta(usuarioAdmin);
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
