using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProyectoInmobiliaria.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Inmueble i)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"INSERT INTO inmuebles (Direccion, TipoInmueble, Estado, Ambientes, Superficie, Longitud, Latitud, Precio, PropietarioId)
                            VALUES (@direccion, @tipo, @estado, @ambientes, @superficie, @longitud, @latitud, @precio, @propietarioId);
                            SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@tipo", i.TipoInmueble);
                    command.Parameters.AddWithValue("@estado", i.Estado);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@superficie", i.Superficie);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);

                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.IdInmueble = res;
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
                var sql = @"DELETE FROM inmuebles WHERE  IdInmueble=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Modificacion(Inmueble i)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"UPDATE inmuebles
                            SET Direccion= @direccion, TipoInmueble= @tipo , Estado =@estado, Ambientes = @ambientes, Superficie = @superficie, 
                            Longitud = @longitud, Latitud = @latitud, Precio = @precio, PropietarioId = @propietarioId, PortadaUrl =@PortadaUrl
                            WHERE IdInmueble=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", i.IdInmueble);
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@tipo", i.TipoInmueble);
                    command.Parameters.AddWithValue("@estado", i.Estado);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@superficie", i.Superficie);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
                    command.Parameters.AddWithValue("@portadaUrl", i.PortadaUrl ?? (object)DBNull.Value);

                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public IList<Inmueble> ObtenerTodos()
        {
            var lista = new List<Inmueble>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT i.IdInmueble, i.Direccion, i.TipoInmueble, i.Estado, 
                           i.Ambientes, i.Superficie, i.Longitud, i.Latitud, i.Precio, i.PropietarioId,i.PortadaUrl,
                           p.Nombre, p.Apellido
                    FROM inmuebles i
                    INNER JOIN propietarios p ON i.PropietarioId = p.PropietarioId";
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                Direccion = reader.GetString("Direccion"),
                                TipoInmueble = reader.IsDBNull(reader.GetOrdinal("TipoInmueble")) ? null : reader.GetString("TipoInmueble"),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString("Estado"),
                                Ambientes = reader.IsDBNull(reader.GetOrdinal("Ambientes")) ? 0 : reader.GetInt32("Ambientes"),
                                Superficie = reader.IsDBNull(reader.GetOrdinal("Superficie")) ? 0 : reader.GetInt32("Superficie"),
                                Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud")) ? 0 : reader.GetInt32("Longitud"),
                                Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud")) ? 0 : reader.GetInt32("Latitud"),
                                Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? 0 : reader.GetDecimal("Precio"),
                                PortadaUrl = reader.IsDBNull(reader.GetOrdinal("PortadaUrl")) ? null : reader.GetString("PortadaUrl"),
                                PropietarioId = reader.GetInt32("PropietarioId"),


                                Propietario = new Propietario
                                {
                                    PropietarioId = reader.GetInt32("PropietarioId"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido")
                                }
                            };

                            lista.Add(inmueble);
                        }
                    }
                }
            }
            return lista;
        }

        public Inmueble ObtenerPorId(int id)
        {
            Inmueble? inmueble = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT IdInmueble, Direccion, TipoInmueble, Estado, Ambientes, Superficie, Longitud, Latitud, Precio, PropietarioId,PortadaUrl
                            FROM inmuebles
                            WHERE IdInmueble=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                Direccion = reader.GetString("Direccion"),
                                TipoInmueble = reader.IsDBNull(reader.GetOrdinal("TipoInmueble")) ? null : reader.GetString("TipoInmueble"),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString("Estado"),
                                Ambientes = reader.IsDBNull(reader.GetOrdinal("Ambientes")) ? 0 : reader.GetInt32("Ambientes"),
                                Superficie = reader.IsDBNull(reader.GetOrdinal("Superficie")) ? 0 : reader.GetInt32("Superficie"),
                                Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud")) ? 0 : reader.GetInt32("Longitud"),
                                Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud")) ? 0 : reader.GetInt32("Latitud"),
                                Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? 0 : reader.GetDecimal("Precio"),
                                PropietarioId = reader.IsDBNull(reader.GetOrdinal("PropietarioId")) ? 0 : reader.GetInt32("PropietarioId"),
                                PortadaUrl = reader.IsDBNull(reader.GetOrdinal("PortadaUrl")) ? null : reader.GetString("PortadaUrl")
                            };
                        }
                    }
                }
            }
            return inmueble!;
        }

        public IList<Inmueble> BuscarPorPropietario(int idPropietario)
        {
            var lista = new List<Inmueble>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT IdInmueble, Direccion, TipoInmueble, Estado, Ambientes ,Superficie, Longitud, Latitud , Precio, PropietarioId
                            FROM inmuebles
                            WHERE PropietarioId =@propietarioId";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@propietarioId", idPropietario);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Inmueble
                            {
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                Direccion = reader.GetString("Direccion"),
                                TipoInmueble = reader.IsDBNull(reader.GetOrdinal("TipoInmueble")) ? null : reader.GetString("TipoInmueble"),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString("Estado"),
                                Ambientes = reader.IsDBNull(reader.GetOrdinal("Ambientes")) ? 0 : reader.GetInt32("Ambientes"),
                                Superficie = reader.IsDBNull(reader.GetOrdinal("Superficie")) ? 0 : reader.GetInt32("Superficie"),
                                Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud")) ? 0 : reader.GetInt32("Longitud"),
                                Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud")) ? 0 : reader.GetInt32("Latitud"),
                                Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? 0 : reader.GetDecimal("Precio"),
                                PropietarioId = reader.IsDBNull(reader.GetOrdinal("PropietarioId")) ? 0 : reader.GetInt32("PropietarioId")
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public bool InmuebleDisponible(int idInmueble)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT COUNT(*) 
                            FROM contratos 
                            WHERE IdInmueble = @idInmueble 
                            AND Estado = 'Activo'";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idInmueble", idInmueble);
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 0;
                }
            }
        }

        public IList<Inmueble> ObtenerDisponibles()
        {
            var lista = new List<Inmueble>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT i.IdInmueble, i.Direccion, i.TipoInmueble, i.Estado,
                                i.Ambientes, i.Superficie, i.Longitud, i.Latitud, i.Precio, 
                                i.PropietarioId, i.PortadaUrl,
                                p.Nombre, p.Apellido
                            FROM inmuebles i
                            INNER JOIN propietarios p ON i.PropietarioId = p.PropietarioId
                            WHERE i.Estado = 'Disponible'";

                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Inmueble
                        {
                            IdInmueble = reader.GetInt32("IdInmueble"),
                            Direccion = reader.GetString("Direccion"),
                            TipoInmueble = reader.IsDBNull(reader.GetOrdinal("TipoInmueble")) ? null : reader.GetString("TipoInmueble"),
                            Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString("Estado"),
                            Ambientes = reader.IsDBNull(reader.GetOrdinal("Ambientes")) ? 0 : reader.GetInt32("Ambientes"),
                            Superficie = reader.IsDBNull(reader.GetOrdinal("Superficie")) ? 0 : reader.GetInt32("Superficie"),
                            Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud")) ? 0 : reader.GetInt32("Longitud"),
                            Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud")) ? 0 : reader.GetInt32("Latitud"),
                            Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? 0 : reader.GetDecimal("Precio"),
                            PortadaUrl = reader.IsDBNull(reader.GetOrdinal("PortadaUrl")) ? null : reader.GetString("PortadaUrl"),
                            PropietarioId = reader.GetInt32("PropietarioId"),
                            Propietario = new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido")
                            }
                        });
                    }
                }
            }
            return lista;
        }

        public IList<Inmueble> ObtenerNoDisponibles()
        {
            var lista = new List<Inmueble>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT i.IdInmueble, i.Direccion, i.TipoInmueble, i.Estado,
                                i.Ambientes, i.Superficie, i.Longitud, i.Latitud, i.Precio, 
                                i.PropietarioId, i.PortadaUrl,
                                p.Nombre, p.Apellido
                            FROM inmuebles i
                            INNER JOIN propietarios p ON i.PropietarioId = p.PropietarioId
                            WHERE i.Estado = 'No Disponible'";

                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Inmueble
                        {
                            IdInmueble = reader.GetInt32("IdInmueble"),
                            Direccion = reader.GetString("Direccion"),
                            TipoInmueble = reader.IsDBNull(reader.GetOrdinal("TipoInmueble")) ? null : reader.GetString("TipoInmueble"),
                            Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString("Estado"),
                            Ambientes = reader.IsDBNull(reader.GetOrdinal("Ambientes")) ? 0 : reader.GetInt32("Ambientes"),
                            Superficie = reader.IsDBNull(reader.GetOrdinal("Superficie")) ? 0 : reader.GetInt32("Superficie"),
                            Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud")) ? 0 : reader.GetInt32("Longitud"),
                            Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud")) ? 0 : reader.GetInt32("Latitud"),
                            Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? 0 : reader.GetDecimal("Precio"),
                            PortadaUrl = reader.IsDBNull(reader.GetOrdinal("PortadaUrl")) ? null : reader.GetString("PortadaUrl"),
                            PropietarioId = reader.GetInt32("PropietarioId"),
                            Propietario = new Propietario
                            {
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido")
                            }
                        });
                    }
                }
            }
            return lista;
        }



        public int ObtenerCantidad()
        {
            int cantidad = 0;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT COUNT(*) FROM inmuebles";
                using (var command = new MySqlCommand(sql, connection))
                {
                    cantidad = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return cantidad;
        }

        public IList<Inmueble> ObtenerLista(int paginaNro, int tamPag)
        {
            var lista = new List<Inmueble>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT i.IdInmueble, i.Direccion, i.TipoInmueble, i.Estado,
                                i.Ambientes, i.Superficie, i.Longitud, i.Latitud, i.Precio,
                                i.PropietarioId, i.PortadaUrl,
                                p.Nombre, p.Apellido
                            FROM inmuebles i
                            INNER JOIN propietarios p ON i.PropietarioId = p.PropietarioId
                            ORDER BY i.IdInmueble
                            LIMIT @limit OFFSET @offset";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@limit", tamPag);
                    command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPag);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                Direccion = reader.GetString("Direccion"),
                                TipoInmueble = reader.IsDBNull(reader.GetOrdinal("TipoInmueble")) ? null : reader.GetString("TipoInmueble"),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString("Estado"),
                                Ambientes = reader.IsDBNull(reader.GetOrdinal("Ambientes")) ? 0 : reader.GetInt32("Ambientes"),
                                Superficie = reader.IsDBNull(reader.GetOrdinal("Superficie")) ? 0 : reader.GetInt32("Superficie"),
                                Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud")) ? 0 : reader.GetInt32("Longitud"),
                                Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud")) ? 0 : reader.GetInt32("Latitud"),
                                Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? 0 : reader.GetDecimal("Precio"),
                                PortadaUrl = reader.IsDBNull(reader.GetOrdinal("PortadaUrl")) ? null : reader.GetString("PortadaUrl"),
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Propietario = new Propietario
                                {
                                    PropietarioId = reader.GetInt32("PropietarioId"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido")
                                }
                            };
                            lista.Add(inmueble);
                        }
                    }
                }
            }
            return lista;
        }
        
        public IList<Inmueble> ObtenerDisponiblesPorFechas(DateOnly fechaInicio, DateOnly fechaFin)
        {
            var lista = new List<Inmueble>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"
                    SELECT i.IdInmueble, i.Direccion, i.TipoInmueble, i.Estado,
                        i.Ambientes, i.Superficie, i.Longitud, i.Latitud, i.Precio,
                        i.PropietarioId, i.PortadaUrl,
                        p.Nombre, p.Apellido
                    FROM inmuebles i
                    INNER JOIN propietarios p ON i.PropietarioId = p.PropietarioId
                    WHERE i.IdInmueble NOT IN (
                        SELECT c.IdInmueble
                        FROM contratos c
                        WHERE (c.FechaInicio <= @fechaFin AND c.FechaFin >= @fechaInicio)
                    )";

                using (var command = new MySqlCommand(sql, connection))
                {
                    // 🔹 Convertimos DateOnly → DateTime porque el parámetro de MySQL lo requiere
                    command.Parameters.AddWithValue("@fechaInicio", fechaInicio.ToDateTime(TimeOnly.MinValue));
                    command.Parameters.AddWithValue("@fechaFin", fechaFin.ToDateTime(TimeOnly.MaxValue));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Inmueble
                            {
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                Direccion = reader.GetString("Direccion"),
                                TipoInmueble = reader.IsDBNull(reader.GetOrdinal("TipoInmueble")) ? null : reader.GetString("TipoInmueble"),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString("Estado"),
                                Ambientes = reader.IsDBNull(reader.GetOrdinal("Ambientes")) ? 0 : reader.GetInt32("Ambientes"),
                                Superficie = reader.IsDBNull(reader.GetOrdinal("Superficie")) ? 0 : reader.GetInt32("Superficie"),
                                Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud")) ? 0 : reader.GetInt32("Longitud"),
                                Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud")) ? 0 : reader.GetInt32("Latitud"),
                                Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? 0 : reader.GetDecimal("Precio"),
                                PortadaUrl = reader.IsDBNull(reader.GetOrdinal("PortadaUrl")) ? null : reader.GetString("PortadaUrl"),
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Propietario = new Propietario
                                {
                                    PropietarioId = reader.GetInt32("PropietarioId"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido")
                                }
                            });
                        }
                    }
                }
            }
            return lista;
        }


    }
}