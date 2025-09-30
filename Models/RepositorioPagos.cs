using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace ProyectoInmobiliaria.Models
{
    public class RepositorioPagos : RepositorioBase, IRepositorioPagos
    {
        public RepositorioPagos(IConfiguration configuration) : base(configuration) { }

        public int Alta(PagosModels p)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = @"INSERT INTO pagos(NroPago, FechaPago, Monto, Detalle, Anulado, IdContrato)
                            VALUES(@nroPago, @fechaPago, @monto, @detalle, 0, @IdContrato);
                            SELECT LAST_INSERT_ID();";

                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@nroPago", p.NroPago);
                command.Parameters.AddWithValue("@fechaPago", p.FechaPago.ToDateTime(new TimeOnly(0, 0)));
                command.Parameters.AddWithValue("@monto", p.Monto);
                command.Parameters.AddWithValue("@detalle", p.Detalle ?? "");
                command.Parameters.AddWithValue("@IdContrato", p.IdContrato);

                int res = Convert.ToInt32(command.ExecuteScalar());
                p.IdPago = res;
                return res;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al crear el pago: {ex.Message}", ex);
            }
        }

        public int Baja(int id)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = @"UPDATE pagos SET Anulado = 1 WHERE IdPago = @id";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al anular el pago con Id {id}: {ex.Message}", ex);
            }
        }

        public int Modificacion(PagosModels p)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = @"UPDATE pagos SET Detalle = @detalle WHERE IdPago = @id";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", p.IdPago);
                command.Parameters.AddWithValue("@detalle", p.Detalle ?? "");
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al modificar el pago con Id {p.IdPago}: {ex.Message}", ex);
            }
        }

        public IList<PagosModels> ObtenerTodos()
        {
            var lista = new List<PagosModels>();
            try
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = @"SELECT IdPago, NroPago, FechaPago, Monto, Detalle, IdContrato, Anulado
                            FROM pagos ORDER BY NroPago";
                using var command = new MySqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new PagosModels
                    {
                        IdPago = reader.GetInt32("IdPago"),
                        NroPago = reader.GetInt32("NroPago"),
                        FechaPago = DateOnly.FromDateTime(reader.GetDateTime("FechaPago")),
                        Monto = reader.GetDecimal("Monto"),
                        Detalle = reader.GetString("Detalle"),
                        IdContrato = reader.GetInt32("IdContrato"),
                        Anulado = reader.GetBoolean("Anulado")
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener todos los pagos: {ex.Message}", ex);
            }

            return lista;
        }

        public PagosModels ObtenerPorId(int id)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = @"SELECT IdPago, NroPago, FechaPago, Monto, Detalle, IdContrato, Anulado
                            FROM pagos WHERE IdPago = @id";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new PagosModels
                    {
                        IdPago = reader.GetInt32("IdPago"),
                        NroPago = reader.GetInt32("NroPago"),
                        FechaPago = DateOnly.FromDateTime(reader.GetDateTime("FechaPago")),
                        Monto = reader.GetDecimal("Monto"),
                        Detalle = reader.GetString("Detalle"),
                        IdContrato = reader.GetInt32("IdContrato"),
                        Anulado = reader.GetBoolean("Anulado")
                    };
                }
                throw new KeyNotFoundException($"No se encontró el pago con Id {id}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener el pago con Id {id}: {ex.Message}", ex);
            }
        }

        public IList<PagosModels> ObtenerPagosPorContrato(int idContrato)
        {
            var lista = new List<PagosModels>();
            try
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = @"SELECT IdPago, NroPago, FechaPago, Monto, Detalle, IdContrato, Anulado
                            FROM pagos WHERE IdContrato = @idContrato ORDER BY NroPago";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@idContrato", idContrato);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new PagosModels
                    {
                        IdPago = reader.GetInt32("IdPago"),
                        NroPago = reader.GetInt32("NroPago"),
                        FechaPago = DateOnly.FromDateTime(reader.GetDateTime("FechaPago")),
                        Monto = reader.GetDecimal("Monto"),
                        Detalle = reader.GetString("Detalle"),
                        IdContrato = reader.GetInt32("IdContrato"),
                        Anulado = reader.GetBoolean("Anulado")
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener pagos del contrato {idContrato}: {ex.Message}", ex);
            }

            return lista;
        }

        public IList<PagosModels> ObtenerLista(int paginaNro, int tamPag)
        {
            var lista = new List<PagosModels>();
            try
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = @"SELECT IdPago, NroPago, FechaPago, Monto, Detalle, IdContrato, Anulado
                            FROM pagos ORDER BY IdPago LIMIT @limit OFFSET @offset";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@limit", tamPag);
                command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPag);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new PagosModels
                    {
                        IdPago = reader.GetInt32("IdPago"),
                        NroPago = reader.GetInt32("NroPago"),
                        FechaPago = DateOnly.FromDateTime(reader.GetDateTime("FechaPago")),
                        Monto = reader.GetDecimal("Monto"),
                        Detalle = reader.GetString("Detalle"),
                        IdContrato = reader.GetInt32("IdContrato"),
                        Anulado = reader.GetBoolean("Anulado")
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener la lista de pagos: {ex.Message}", ex);
            }

            return lista;
        }

        public int ObtenerCantidad()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = @"SELECT COUNT(*) FROM pagos";
                using var command = new MySqlCommand(sql, connection);
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener la cantidad de pagos: {ex.Message}", ex);
            }
        }

        public int ObtenerUltimoNumeroPago(int idContrato)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                var sql = @"SELECT MAX(NroPago) FROM pagos WHERE IdContrato = @idContrato AND Anulado = 0";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@idContrato", idContrato);

                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener el último número de pago del contrato {idContrato}: {ex.Message}", ex);
            }
        }

        public void AnularPago(int idPago)
        {
            if (idPago <= 0) throw new ArgumentException("Id de pago no válido.");
            Baja(idPago);
        }
    }
}
