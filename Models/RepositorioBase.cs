

using MySql.Data.MySqlClient;

namespace ProyectoInmobiliaria.Models
{
    public abstract class RepositorioBase
    {
        protected readonly string? connectionString;

        protected RepositorioBase(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
