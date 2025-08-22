using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProyectoInmobiliaria.Models
{
    public class RepositorioInquilino : RepositorioBase, IRepositorioInquilino
    {
        public RepositorioInquilino(IConfiguration configuration) : base(configuration)
        {

        }
        
        public int Alta(Inquilino i)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"INSERT INTO Inquilinos (Nombre, Apellido, Dni, Email, Telefono)
                            VALUES (@nombre,@apellido,@dni,@email,@telefono);
                            SELECT LAST_INSERT_ID()";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", i.Nombre);
                    command.Parameters.AddWithValue("@apellido", i.Apellido);
                    command.Parameters.AddWithValue("@Dni", i.Dni);
                    command.Parameters.AddWithValue("@email", i.Email);
                    command.Parameters.AddWithValue("@telefono", i.Telefono);
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.InquilinoId = res;
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
                var sql = @"DELETE FROM Inquilinos WHERE InquilinoId = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Modificacion(Inquilino i)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"UPDATE Inquilinos
                            SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Email=@email, Telefono=@telefono
                            WHERE InquilinoId=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", i.InquilinoId);
                    command.Parameters.AddWithValue("@nombre", i.Nombre);
                    command.Parameters.AddWithValue("@apellido", i.Apellido);
                    command.Parameters.AddWithValue("@dni", i.Dni);
                    command.Parameters.AddWithValue("@email", i.Email);
                    command.Parameters.AddWithValue("@telefono", i.Telefono);
                    res = command.ExecuteNonQuery();
                }

            }
            return res;
        }

        public IList<Inquilino> ObtenerTodos()
        {
            var lista = new List<Inquilino>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT InquilinoId, Nombre, Apellido, Dni, Email, Telefono
                            FROM Inquilinos";
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Inquilino
                            {
                                InquilinoId = reader.GetInt32("InquilinoId"),
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

        public Inquilino ObtenerPorId(int id)
        {
            Inquilino? inquilino = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT InquilinoId, Nombre, Apellido, Email, Dni, Telefono
                            FROM Inquilinos
                            WHERE InquilinoId=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inquilino = new Inquilino
                            {
                                InquilinoId = reader.GetInt32("InquilinoId"),
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
            return inquilino!;
        }

        public Inquilino ObtenerPorEmail(string email)
        {
            Inquilino? Inquilino = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT InquilinoId, Nombre, Apellido, Email, Dni, Telefono
                            FROM Inquilinos
                            WHERE Email=@email";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Inquilino = new Inquilino
                            {
                                InquilinoId = reader.GetInt32("InquilinoId"),
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
            return Inquilino!;
        }

        public int ObtenerCantidad()
        {
            int cantidad = 0;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT COUNT(*) FROM Inquilinos";
                using (var command = new MySqlCommand(sql, connection))
                {
                    cantidad = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return cantidad;
        }

        public IList<Inquilino> BuscarPorNombre(string nombre)
        {
            var lista = new List<Inquilino>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT InquilinoId, Nombre, Apellido, Dni, Email, Telefono
                            FROM Inquilinos
                            WHERE Nombre LIKE @nombre OR Apellido LIKE @nombre";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", "%" + nombre + "%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Inquilino
                            {
                                InquilinoId = reader.GetInt32("InquilinoId"),
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


        public IList<Inquilino> ObtenerLista(int paginaNro, int tamPag)
        {
            var lista = new List<Inquilino>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT InquilinoId, Nombre, Apellido, Dni, Email, Telefono
                            FROM Inquilinos
                            ORDER BY InquilinoId
                            LIMIT @limit OFFSET @offset";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@limit", tamPag);
                    command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPag);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Inquilino
                            {
                                InquilinoId = reader.GetInt32("InquilinoId"),
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
