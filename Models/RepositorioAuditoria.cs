using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace ProyectoInmobiliaria.Models
{
    public class RepositorioAuditoria : RepositorioBase, IRepositorioAuditoria
    {
        public RepositorioAuditoria(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(AuditoriaModel auditoria)
        {
            int res = -1;

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"INSERT INTO auditorias 
                            (Entidad, EntidadId, UsuarioId, Fecha, Accion, Datos, Detalle)
                            VALUES (@Entidad, @EntidadId, @UsuarioId, @Fecha, @Accion, @Datos, @Detalle);
                            SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Entidad", auditoria.Entidad);
                    command.Parameters.AddWithValue("@EntidadId", auditoria.EntidadId);
                    command.Parameters.AddWithValue("@UsuarioId", auditoria.UsuarioId);
                    command.Parameters.AddWithValue("@Fecha", auditoria.Fecha);
                    command.Parameters.AddWithValue("@Accion", auditoria.Accion);
                    command.Parameters.AddWithValue("@Datos", auditoria.Datos ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Detalle", auditoria.Detalle ?? (object)DBNull.Value);

                    res = Convert.ToInt32(command.ExecuteScalar());
                    auditoria.IdAuditoria = res;
                }
            }

            return res;
        }

        public List<AuditoriaModel> ObtenerTodos()
        {
            var lista = new List<AuditoriaModel>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT IdAuditoria, Entidad, EntidadId, UsuarioId, Fecha, Accion, Datos, Detalle
                            FROM auditorias
                            ORDER BY Fecha DESC";

                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new AuditoriaModel
                        {
                            IdAuditoria = reader.GetInt32("IdAuditoria"),
                            Entidad = reader.GetString("Entidad"),
                            EntidadId = reader.GetInt32("EntidadId"),
                            UsuarioId = reader.GetInt32("UsuarioId"),
                            Fecha = reader.GetDateTime("Fecha"),
                            Accion = reader.GetString("Accion"),
                            Datos = reader.IsDBNull(reader.GetOrdinal("Datos")) ? null : reader.GetString("Datos"),
                            Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle")) ? null : reader.GetString("Detalle")
                        });
                    }
                }
            }

            return lista;
        }

        public List<AuditoriaModel> ObtenerPorEntidad(string entidad, int entidadId)
        {
            var lista = new List<AuditoriaModel>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT IdAuditoria, Entidad, EntidadId, UsuarioId, Fecha, Accion, Datos, Detalle
                            FROM auditorias
                            WHERE Entidad = @Entidad AND EntidadId = @EntidadId
                            ORDER BY Fecha DESC";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Entidad", entidad);
                    command.Parameters.AddWithValue("@EntidadId", entidadId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new AuditoriaModel
                            {
                                IdAuditoria = reader.GetInt32("IdAuditoria"),
                                Entidad = reader.GetString("Entidad"),
                                EntidadId = reader.GetInt32("EntidadId"),
                                UsuarioId = reader.GetInt32("UsuarioId"),
                                Fecha = reader.GetDateTime("Fecha"),
                                Accion = reader.GetString("Accion"),
                                Datos = reader.IsDBNull(reader.GetOrdinal("Datos")) ? null : reader.GetString("Datos"),
                                Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle")) ? null : reader.GetString("Detalle")
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public AuditoriaModel? ObtenerPorId(int id)
        {
            AuditoriaModel? auditoria = null;

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT IdAuditoria, Entidad, EntidadId, UsuarioId, Fecha, Accion, Datos, Detalle
                            FROM auditorias
                            WHERE IdAuditoria = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            auditoria = new AuditoriaModel
                            {
                                IdAuditoria = reader.GetInt32("IdAuditoria"),
                                Entidad = reader.GetString("Entidad"),
                                EntidadId = reader.GetInt32("EntidadId"),
                                UsuarioId = reader.GetInt32("UsuarioId"),
                                Fecha = reader.GetDateTime("Fecha"),
                                Accion = reader.GetString("Accion"),
                                Datos = reader.IsDBNull(reader.GetOrdinal("Datos")) ? null : reader.GetString("Datos"),
                                Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle")) ? null : reader.GetString("Detalle")
                            };
                        }
                    }
                }
            }

            return auditoria;
        }

        // Método estilo ObtenerLista con paginación
        public List<AuditoriaModel> ObtenerLista(int paginaNro, int tamPag)
        {
            var lista = new List<AuditoriaModel>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT IdAuditoria, Entidad, EntidadId, UsuarioId, Fecha, Accion, Datos, Detalle
                            FROM auditorias
                            ORDER BY Fecha DESC
                            LIMIT @limit OFFSET @offset";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@limit", tamPag);
                    command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPag);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new AuditoriaModel
                            {
                                IdAuditoria = reader.GetInt32("IdAuditoria"),
                                Entidad = reader.GetString("Entidad"),
                                EntidadId = reader.GetInt32("EntidadId"),
                                UsuarioId = reader.GetInt32("UsuarioId"),
                                Fecha = reader.GetDateTime("Fecha"),
                                Accion = reader.GetString("Accion"),
                                Datos = reader.IsDBNull(reader.GetOrdinal("Datos")) ? null : reader.GetString("Datos"),
                                Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle")) ? null : reader.GetString("Detalle")
                            });
                        }
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
                var sql = @"SELECT COUNT(*) FROM auditorias";
                using (var command = new MySqlCommand(sql, connection))
                {
                    cantidad = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return cantidad;
        }

        public Dictionary<int, string> ObtenerUsuariosPorIds(List<int> ids)
        {
            var resultado = new Dictionary<int, string>();

            if (ids == null || ids.Count == 0)
                return resultado;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string idParams = string.Join(",", ids.Select((id, i) => $"@id{i}"));
                string sql = $"SELECT IdUsuario, CONCAT(Nombre, ' ', Apellido) AS NombreCompleto FROM Usuarios WHERE IdUsuario IN ({idParams})";

                using (var command = new MySqlCommand(sql, connection))
                {
                    for (int i = 0; i < ids.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@id{i}", ids[i]);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idUsuario = reader["IdUsuario"] != DBNull.Value 
                                ? Convert.ToInt32(reader["IdUsuario"]) 
                                : 0;

                            string nombre = reader["NombreCompleto"] != DBNull.Value 
                                ? reader["NombreCompleto"]!.ToString()! 
                                : "Desconocido";

                            resultado[idUsuario] = nombre;
                        }
                    }
                }
            }

            return resultado;
        }




    }
}
