using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProyectoInmobiliaria.Models
{
    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Contrato c)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"INSERT INTO contratos (FechaInicio, FechaFin, Precio , Estado, InquilinoId, IdInmueble)
                            VALUES (@FechaInicio, @FechaFin,@Precio,@Estado,@InquilinoId,@InmuebleId);
                            SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FechaInicio", c.FechaInicio.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@FechaFin", c.FechaFin.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Precio", c.Precio);
                    command.Parameters.AddWithValue("@Estado", c.Estado);
                    command.Parameters.AddWithValue("@InquilinoId", c.InquilinoId);
                    command.Parameters.AddWithValue("@InmuebleId", c.IdInmueble);

                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.IdContrato = res;

                }

                var sqlUpdateInmueble = @"UPDATE inmuebles SET Estado ='Alquilado' WHERE IdInmueble = @inmuebleId";
                using (var commandUpdate = new MySqlCommand(sqlUpdateInmueble, connection))
                {
                    commandUpdate.Parameters.AddWithValue("@InmuebleId", c.IdInmueble);
                    commandUpdate.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            int idInmueble = -1;

            using (var connection = GetConnection())
            {
                connection.Open();


                var sqlSelect = "SELECT IdInmueble FROM contratos WHERE IdContrato = @id";
                using (var commandSelect = new MySqlCommand(sqlSelect, connection))
                {
                    commandSelect.Parameters.AddWithValue("@id", id);
                    var result = commandSelect.ExecuteScalar();
                    if (result != null)
                        idInmueble = Convert.ToInt32(result);
                }


                var sqlDelete = "DELETE FROM contratos WHERE IdContrato = @id";
                using (var commandDelete = new MySqlCommand(sqlDelete, connection))
                {
                    commandDelete.Parameters.AddWithValue("@id", id);
                    res = commandDelete.ExecuteNonQuery();
                }


                if (res > 0 && idInmueble != -1)
                {
                    var sqlUpdate = "UPDATE inmuebles SET Estado = 'Disponible' WHERE IdInmueble = @idInmueble";
                    using (var commandUpdate = new MySqlCommand(sqlUpdate, connection))
                    {
                        commandUpdate.Parameters.AddWithValue("@idInmueble", idInmueble);
                        commandUpdate.ExecuteNonQuery();
                    }
                }
            }

            return res;
        }

        public int Modificacion(Contrato c)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"UPDATE contratos
                            SET FechaInicio = @fechaInicio, FechaFin = @fechaFin , Precio =@precio, Estado =@estado, InquilinoId=@inquilinoId, IdInmueble=@inmuebleId
                            WHERE IdContrato =@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", c.IdContrato);
                    command.Parameters.AddWithValue("@fechaInicio", c.FechaInicio.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@fechaFin", c.FechaFin.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@precio", c.Precio);
                    command.Parameters.AddWithValue("@estado", c.Estado);
                    command.Parameters.AddWithValue("@inquilinoId", c.InquilinoId);
                    command.Parameters.AddWithValue("@inmuebleId", c.IdInmueble);

                    res = command.ExecuteNonQuery();

                }
            }
            return res;
        }


        public IList<Contrato> ObtenerTodos()
        {
            var lista = new List<Contrato>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"
                    SELECT 
                        c.IdContrato, c.FechaInicio, c.FechaFin, c.Precio, c.Estado,
                        i.InquilinoId, i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                        im.IdInmueble, im.Direccion AS DireccionInmueble,
                        p.PropietarioId, p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
                    FROM contratos c
                    INNER JOIN inquilinos i ON c.InquilinoId = i.InquilinoId
                    INNER JOIN inmuebles im ON c.IdInmueble = im.IdInmueble
                    INNER JOIN propietarios p ON im.PropietarioId = p.PropietarioId";

                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contrato = new Contrato
                        {
                            IdContrato = reader.GetInt32("IdContrato"),
                            FechaInicio = DateOnly.FromDateTime(reader.GetDateTime("FechaInicio")),
                            FechaFin = DateOnly.FromDateTime(reader.GetDateTime("FechaFin")),
                            Precio = reader.GetDecimal("Precio"),
                            Estado = reader.GetString("Estado"),

                            Inquilino = new Inquilino
                            {
                                InquilinoId = reader.GetInt32("InquilinoId"),
                                Nombre = reader.GetString("NombreInquilino"),
                                Apellido = reader.GetString("ApellidoInquilino")
                            },

                            Inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                Direccion = reader.GetString("DireccionInmueble"),

                                Propietario = new Propietario
                                {
                                    PropietarioId = reader.GetInt32("PropietarioId"),
                                    Nombre = reader.GetString("NombrePropietario"),
                                    Apellido = reader.GetString("ApellidoPropietario")
                                }
                            }
                        };

                        lista.Add(contrato);
                    }
                }
            }

            return lista;
        }

        public Contrato ObtenerPorId(int id)
        {
            Contrato? contrato = null;

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"
                    SELECT 
                        c.IdContrato, c.FechaInicio, c.FechaFin, c.Precio, c.Estado,
                        i.InquilinoId, i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                        im.IdInmueble, im.Direccion AS DireccionInmueble,
                        p.PropietarioId, p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
                    FROM contratos c
                    INNER JOIN inquilinos i ON c.InquilinoId = i.InquilinoId
                    INNER JOIN inmuebles im ON c.IdInmueble = im.IdInmueble
                    INNER JOIN propietarios p ON im.PropietarioId = p.PropietarioId
                    WHERE c.IdContrato = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32("IdContrato"),
                                FechaInicio = DateOnly.FromDateTime(reader.GetDateTime("FechaInicio")),
                                FechaFin = DateOnly.FromDateTime(reader.GetDateTime("FechaFin")),
                                Precio = reader.GetDecimal("Precio"),
                                Estado = reader.GetString("Estado"),
                                InquilinoId = reader.GetInt32("InquilinoId"),
                                IdInmueble = reader.GetInt32("IdInmueble"),

                                Inquilino = new Inquilino
                                {
                                    InquilinoId = reader.GetInt32("InquilinoId"),
                                    Nombre = reader.GetString("NombreInquilino"),
                                    Apellido = reader.GetString("ApellidoInquilino")
                                },

                                Inmueble = new Inmueble
                                {
                                    IdInmueble = reader.GetInt32("IdInmueble"),
                                    Direccion = reader.GetString("DireccionInmueble"),

                                    Propietario = new Propietario
                                    {
                                        PropietarioId = reader.GetInt32("PropietarioId"),
                                        Nombre = reader.GetString("NombrePropietario"),
                                        Apellido = reader.GetString("ApellidoPropietario")
                                    }
                                }
                            };
                        }
                    }
                }
            }

            return contrato!;
        }


        public IList<Contrato> BuscarContratosActivos()
        {
            var lista = new List<Contrato>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"
                    SELECT 
                        c.IdContrato, c.FechaInicio, c.FechaFin, c.Precio, c.Estado,
                        i.InquilinoId, i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                        im.IdInmueble, im.Direccion AS DireccionInmueble,
                        p.PropietarioId, p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
                    FROM contratos c
                    INNER JOIN inquilinos i ON c.InquilinoId = i.InquilinoId
                    INNER JOIN inmuebles im ON c.IdInmueble = im.IdInmueble
                    INNER JOIN propietarios p ON im.PropietarioId = p.PropietarioId
                    WHERE c.Estado = 'Activo'";

                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contrato = new Contrato
                        {
                            IdContrato = reader.GetInt32("IdContrato"),
                            FechaInicio = DateOnly.FromDateTime(reader.GetDateTime("FechaInicio")),
                            FechaFin = DateOnly.FromDateTime(reader.GetDateTime("FechaFin")),
                            Precio = reader.GetDecimal("Precio"),
                            Estado = reader.GetString("Estado"),

                            Inquilino = new Inquilino
                            {
                                InquilinoId = reader.GetInt32("InquilinoId"),
                                Nombre = reader.GetString("NombreInquilino"),
                                Apellido = reader.GetString("ApellidoInquilino")
                            },

                            Inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                Direccion = reader.GetString("DireccionInmueble"),

                                Propietario = new Propietario
                                {
                                    PropietarioId = reader.GetInt32("PropietarioId"),
                                    Nombre = reader.GetString("NombrePropietario"),
                                    Apellido = reader.GetString("ApellidoPropietario")
                                }
                            }
                        };

                        lista.Add(contrato);
                    }
                }
            }

            return lista;
        }


        public IList<Contrato> BuscarPorInquilino(int inquilinoId)
        {
            var lista = new List<Contrato>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"
                    SELECT 
                        c.IdContrato, c.FechaInicio, c.FechaFin, c.Precio, c.Estado,
                        i.InquilinoId, i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                        im.IdInmueble, im.Direccion AS DireccionInmueble,
                        p.PropietarioId, p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
                    FROM contratos c
                    INNER JOIN inquilinos i ON c.InquilinoId = i.InquilinoId
                    INNER JOIN inmuebles im ON c.IdInmueble = im.IdInmueble
                    INNER JOIN propietarios p ON im.PropietarioId = p.PropietarioId
                    WHERE c.InquilinoId = @inquilinoId";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@inquilinoId", inquilinoId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32("IdContrato"),
                                FechaInicio = DateOnly.FromDateTime(reader.GetDateTime("FechaInicio")),
                                FechaFin = DateOnly.FromDateTime(reader.GetDateTime("FechaFin")),
                                Precio = reader.GetDecimal("Precio"),
                                Estado = reader.GetString("Estado"),

                                Inquilino = new Inquilino
                                {
                                    InquilinoId = reader.GetInt32("InquilinoId"),
                                    Nombre = reader.GetString("NombreInquilino"),
                                    Apellido = reader.GetString("ApellidoInquilino")
                                },

                                Inmueble = new Inmueble
                                {
                                    IdInmueble = reader.GetInt32("IdInmueble"),
                                    Direccion = reader.GetString("DireccionInmueble"),

                                    Propietario = new Propietario
                                    {
                                        PropietarioId = reader.GetInt32("PropietarioId"),
                                        Nombre = reader.GetString("NombrePropietario"),
                                        Apellido = reader.GetString("ApellidoPropietario")
                                    }
                                }
                            };

                            lista.Add(contrato);
                        }
                    }
                }
            }

            return lista;
        }

        public IList<Contrato> BuscarPorPropietario(int propietarioId)
        {
            var lista = new List<Contrato>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"
                    SELECT 
                        c.IdContrato, c.FechaInicio, c.FechaFin, c.Precio, c.Estado,
                        i.InquilinoId, i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                        im.IdInmueble, im.Direccion AS DireccionInmueble,
                        p.PropietarioId, p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
                    FROM contratos c
                    INNER JOIN inquilinos i ON c.InquilinoId = i.InquilinoId
                    INNER JOIN inmuebles im ON c.IdInmueble = im.IdInmueble
                    INNER JOIN propietarios p ON im.PropietarioId = p.PropietarioId
                    WHERE p.PropietarioId = @propietarioId";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@propietarioId", propietarioId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32("IdContrato"),
                                FechaInicio = DateOnly.FromDateTime(reader.GetDateTime("FechaInicio")),
                                FechaFin = DateOnly.FromDateTime(reader.GetDateTime("FechaFin")),
                                Precio = reader.GetDecimal("Precio"),
                                Estado = reader.GetString("Estado"),

                                Inquilino = new Inquilino
                                {
                                    InquilinoId = reader.GetInt32("InquilinoId"),
                                    Nombre = reader.GetString("NombreInquilino"),
                                    Apellido = reader.GetString("ApellidoInquilino")
                                },

                                Inmueble = new Inmueble
                                {
                                    IdInmueble = reader.GetInt32("IdInmueble"),
                                    Direccion = reader.GetString("DireccionInmueble"),

                                    Propietario = new Propietario
                                    {
                                        PropietarioId = reader.GetInt32("PropietarioId"),
                                        Nombre = reader.GetString("NombrePropietario"),
                                        Apellido = reader.GetString("ApellidoPropietario")
                                    }
                                }
                            };

                            lista.Add(contrato);
                        }
                    }
                }
            }

            return lista;
        }

        public int TerminarAnticipado(int id, DateOnly fechaAnticipada, decimal multa)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                string sql = @"UPDATE contratos 
                            SET Estado = 'Finalizado',
                                FechaAnticipada = @fechaAnticipada,
                                Multa = @multa
                            WHERE IdContrato = @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaAnticipada", fechaAnticipada.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@multa", multa);
                    command.Parameters.AddWithValue("@id", id);

                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int ObtenerCantidad()
        {
            int cantidad = 0;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT COUNT(*) FROM contratos";
                using (var command = new MySqlCommand(sql, connection))
                {
                    cantidad = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return cantidad;
        }

        public IList<Contrato> ObtenerLista(int paginaNro, int tamPag)
        {
            var lista = new List<Contrato>();
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var sql = @"
                        SELECT 
                            c.IdContrato, c.FechaInicio, c.FechaFin, c.Precio, c.Estado,
                            i.InquilinoId, i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                            im.IdInmueble, im.Direccion AS DireccionInmueble,
                            p.PropietarioId, p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
                        FROM contratos c
                        INNER JOIN inquilinos i ON c.InquilinoId = i.InquilinoId
                        INNER JOIN inmuebles im ON c.IdInmueble = im.IdInmueble
                        INNER JOIN propietarios p ON im.PropietarioId = p.PropietarioId
                        ORDER BY c.IdContrato
                        LIMIT @limit OFFSET @offset";

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@limit", tamPag);
                        command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPag);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new Contrato
                                {
                                    IdContrato = reader.GetInt32("IdContrato"),
                                    FechaInicio = DateOnly.FromDateTime(reader.GetDateTime("FechaInicio")),
                                    FechaFin = DateOnly.FromDateTime(reader.GetDateTime("FechaFin")),
                                    Precio = reader.GetDecimal("Precio"),
                                    Estado = reader.GetString("Estado"),

                                    Inquilino = new Inquilino
                                    {
                                        InquilinoId = reader.GetInt32("InquilinoId"),
                                        Nombre = reader.GetString("NombreInquilino"),
                                        Apellido = reader.GetString("ApellidoInquilino")
                                    },

                                    Inmueble = new Inmueble
                                    {
                                        IdInmueble = reader.GetInt32("IdInmueble"),
                                        Direccion = reader.GetString("DireccionInmueble"),

                                        Propietario = new Propietario
                                        {
                                            PropietarioId = reader.GetInt32("PropietarioId"),
                                            Nombre = reader.GetString("NombrePropietario"),
                                            Apellido = reader.GetString("ApellidoPropietario")
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en ObtenerLista Contratos: " + ex.Message);
            }

            return lista;
        }

        public int RenovarContrato(int idContrato)
        {
            int nuevoId = -1;

            using (var connection = GetConnection())
            {
                connection.Open();

                var contrato = ObtenerPorId(idContrato);
                if (contrato == null) return -1;

                DateTime nuevaFechaInicio = contrato.FechaFin.ToDateTime(TimeOnly.MinValue).AddDays(1);
                DateTime nuevaFechaFin = nuevaFechaInicio.AddMonths(6);

                string sql = @"INSERT INTO contratos 
                                (FechaInicio, FechaFin, Precio, Estado, InquilinoId, IdInmueble)
                            VALUES (@fechaInicio, @fechaFin, @precio, 'Activo', @inquilinoId, @idInmueble);
                            SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaInicio", nuevaFechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", nuevaFechaFin);
                    command.Parameters.AddWithValue("@precio", contrato.Precio);
                    command.Parameters.AddWithValue("@inquilinoId", contrato.InquilinoId);
                    command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);

                    nuevoId = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return nuevoId;
        }

        public bool ExisteSolapamiento(int idInmueble, DateOnly inicio, DateOnly fin, int? idContrato = null)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"
                    SELECT COUNT(*) 
                    FROM contratos 
                    WHERE IdInmueble = @idInmueble
                    AND Estado = 'Activo'
                    AND (
                            (@inicio <= FechaFin AND @fin >= FechaInicio)
                        )
                ";

                if (idContrato.HasValue)
                    sql += " AND IdContrato <> @idContrato";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idInmueble", idInmueble);
                    command.Parameters.AddWithValue("@inicio", inicio.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@fin", fin.ToString("yyyy-MM-dd"));
                    if (idContrato.HasValue)
                        command.Parameters.AddWithValue("@idContrato", idContrato.Value);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0; 
                }
            }
        }

     }


}
