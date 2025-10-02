using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProyectoInmobiliaria.Models
{
    public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Usuario u)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"INSERT INTO usuarios (Nombre, Apellido, Email, PasswordHash, Rol, Avatar)
                            VALUES (@Nombre, @Apellido, @Email, @PasswordHash, @Rol, @Avatar);
                            SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", u.Nombre);
                    command.Parameters.AddWithValue("@Apellido", u.Apellido);
                    command.Parameters.AddWithValue("@Email", u.Email);
                    command.Parameters.AddWithValue("@PasswordHash", u.PasswordHash);
                    command.Parameters.AddWithValue("@Rol", u.Rol);
                    command.Parameters.AddWithValue("@Avatar", u.Avatar ?? "/img/default-avatar.png");

                    res = Convert.ToInt32(command.ExecuteScalar());
                    u.IdUsuario = res;
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = "DELETE FROM usuarios WHERE IdUsuario = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Modificacion(Usuario u)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"UPDATE usuarios
                            SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, 
                                PasswordHash = @PasswordHash, Rol = @Rol, Avatar = @Avatar
                            WHERE IdUsuario = @IdUsuario";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", u.IdUsuario);
                    command.Parameters.AddWithValue("@Nombre", u.Nombre);
                    command.Parameters.AddWithValue("@Apellido", u.Apellido);
                    command.Parameters.AddWithValue("@Email", u.Email);
                    command.Parameters.AddWithValue("@PasswordHash", u.PasswordHash);
                    command.Parameters.AddWithValue("@Rol", u.Rol);
                    command.Parameters.AddWithValue("@Avatar", u.Avatar ?? "/img/default-avatar.png");

                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public IList<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = "SELECT IdUsuario, Nombre, Apellido, Email, PasswordHash, Rol, Avatar FROM usuarios";
                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuario
                        {
                            IdUsuario = reader.GetInt32("IdUsuario"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Email = reader.GetString("Email"),
                            PasswordHash = reader.GetString("PasswordHash"),
                            Rol = reader.GetString("Rol"),
                            Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar"))
                                ? "/img/default-avatar.png"
                                : reader.GetString("Avatar")
                        });
                    }
                }
            }
            return lista;
        }

        public Usuario ObtenerPorId(int id)
        {
            Usuario? usuario = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = "SELECT IdUsuario, Nombre, Apellido, Email, PasswordHash, Rol, Avatar FROM usuarios WHERE IdUsuario = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32("IdUsuario"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Email = reader.GetString("Email"),
                                PasswordHash = reader.GetString("PasswordHash"),
                                Rol = reader.GetString("Rol"),
                                Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar"))
                                    ? "/img/default-avatar.png"
                                    : reader.GetString("Avatar")
                            };
                        }
                    }
                }
            }
            return usuario!;
        }

        public Usuario ObtenerPorEmail(string email)
        {
            Usuario? usuario = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = "SELECT IdUsuario, Nombre, Apellido, Email, PasswordHash, Rol, Avatar FROM usuarios WHERE Email = @Email";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32("IdUsuario"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Email = reader.GetString("Email"),
                                PasswordHash = reader.GetString("PasswordHash"),
                                Rol = reader.GetString("Rol"),
                                Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar"))
                                    ? "/img/default-avatar.png"
                                    : reader.GetString("Avatar")
                            };
                        }
                    }
                }
            }
            return usuario!;
        }

        public int ObtenerCantidad()
        {
            int cantidad = 0;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = "SELECT COUNT(*) FROM usuarios";
                using (var command = new MySqlCommand(sql, connection))
                {
                    cantidad = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return cantidad;
        }

        public IList<Usuario> ObtenerLista(int paginaNro, int tamPag)
        {
            var lista = new List<Usuario>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT IdUsuario, Nombre, Apellido, Email, PasswordHash, Rol, Avatar
                            FROM usuarios
                            ORDER BY IdUsuario
                            LIMIT @limit OFFSET @offset";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@limit", tamPag);
                    command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPag);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Usuario
                            {
                                IdUsuario = reader.GetInt32("IdUsuario"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Email = reader.GetString("Email"),
                                PasswordHash = reader.GetString("PasswordHash"),
                                Rol = reader.GetString("Rol"),
                                Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar"))
                                    ? "/img/default-avatar.png"
                                    : reader.GetString("Avatar")
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
