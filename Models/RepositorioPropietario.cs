using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProyectoInmobiliaria.Models
{
    public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
    {
        public RepositorioPropietario(IConfiguration configuration) : base(configuration)
        {

        }
        public int Alta(Propietario p)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"INSERT INTO Propietarios (Nombre, Apellido, Dni, Email, Telefono)
                            VALUES (@nombre,@apellido,@dni,@email,@telefono);
                            SELECT LAST_INSERT_ID()";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@Dni", p.Dni);
                    command.Parameters.AddWithValue("@email", p.Email);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.PropietarioId = res;
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
                var sql = @"DELETE FROM Propietarios WHERE PropietarioId = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Modificacion(Propietario p)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"UPDATE Propietarios
                            SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Email=@email, Telefono=@telefono
                            WHERE PropietarioId=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", p.PropietarioId);
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@email", p.Email);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);
                    res = command.ExecuteNonQuery();
                }

            }
            return res;
        }

        public IList<Propietario> ObtenerTodos()
        {
            var lista = new List<Propietario>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT PropietarioId, Nombre, Apellido, Dni, Email, Telefono
                            FROM Propietarios";
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Dni = reader.GetString("Dni"),
                                Email = reader.GetString("Email"),
                                Telefono = reader.GetString("Telefono")


                            });
                        }
                    }
                }
            }
            return lista;
        }

        public Propietario ObtenerPorId(int id)
        {
            Propietario? propietario = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT PropietarioId, Nombre, Apellido, Email, Dni, Telefono
                            FROM Propietarios
                            WHERE PropietarioId=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            propietario = new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Dni = reader.GetString("Dni"),
                                Email = reader.GetString("Email"),
                                Telefono = reader.GetString("Telefono")
                            };
                        }
                    }
                }
            }
            return propietario!;
        }

        public Propietario ObtenerPorEmail(string email)
        {
            Propietario? propietario = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT PropietarioId, Nombre, Apellido, Email, Dni, Telefono
                            FROM Propietarios
                            WHERE Email=@email";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            propietario = new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Dni = reader.GetString("Dni"),
                                Email = reader.GetString("Email"),
                                Telefono = reader.GetString("Telefono")
                            };
                        }
                    }
                }
            }
            return propietario!;
        }

        public int ObtenerCantidad()
        {
            int cantidad = 0;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT COUNT(*) FROM Propietarios";
                using (var command = new MySqlCommand(sql, connection))
                {
                    cantidad = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return cantidad;
        }

        public IList<Propietario> BuscarPorNombre(string nombre)
        {
            var lista = new List<Propietario>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT PropietarioId, Nombre, Apellido, Dni, Email, Telefono
                            FROM Propietarios
                            WHERE Nombre LIKE @nombre OR Apellido LIKE @nombre";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", "%" + nombre + "%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Dni = reader.GetString("Dni"),
                                Email = reader.GetString("Email"),
                                Telefono = reader.GetString("Telefono")
                            });
                        }
                    }
                }


            }
            return lista;
        }


        public IList<Propietario> ObtenerLista(int paginaNro, int tamPag)
        {
            var lista = new List<Propietario>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT PropietarioId, Nombre, Apellido, Dni, Email, Telefono
                            FROM Propietarios
                            ORDER BY PropietarioId
                            LIMIT @limit OFFSET @offset";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@limit", tamPag);
                    command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPag);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Dni = reader.GetString("Dni"),
                                Email = reader.GetString("Email"),
                                Telefono = reader.GetString("Telefono")
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }

   
}