using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ProyectoInmobiliaria.Models
{
    public class RepositorioPagos : RepositorioBase, IRepositorioPagos
    {
        public RepositorioPagos(IConfiguration configuration) : base(configuration) { }

        public int Alta(PagosModels p)
        {
            int res = -1;
            using var connection = GetConnection();
            connection.Open();

            var sql = @"INSERT INTO pagos(NroPago, FechaPago, Monto, Detalle, Anulado, IdContrato)
                        VALUES(@nroPago, @fechaPago, @monto, @detalle, 0, @IdContrato);
                        SELECT LAST_INSERT_ID();";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nroPago", p.NroPago);
            command.Parameters.AddWithValue("@fechaPago", p.FechaPago.ToDateTime(new TimeOnly(0, 0))); // ✅ CORRECTO
            command.Parameters.AddWithValue("@monto", p.Monto);
            command.Parameters.AddWithValue("@detalle", p.Detalle);
            command.Parameters.AddWithValue("@IdContrato", p.IdContrato);

            res = Convert.ToInt32(command.ExecuteScalar());
            p.IdPago = res;

            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using var connection = GetConnection();
            connection.Open();

            var sql = @"UPDATE pagos SET Anulado = 1 WHERE IdPago = @id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            res = command.ExecuteNonQuery();

            return res;
        }

        public int Modificacion(PagosModels p)
        {
            int res = -1;
            using var connection = GetConnection();
            connection.Open();

            var sql = @"UPDATE pagos SET Detalle = @detalle WHERE IdPago = @id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", p.IdPago);
            command.Parameters.AddWithValue("@detalle", p.Detalle ?? "");
            res = command.ExecuteNonQuery();

            return res;
        }

        public IList<PagosModels> ObtenerTodos()
        {
            var lista = new List<PagosModels>();
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
                    FechaPago = DateOnly.FromDateTime(reader.GetDateTime("FechaPago")), // ✅ Leer DateTime y convertir
                    Monto = reader.GetDecimal("Monto"),
                    Detalle = reader.GetString("Detalle"),
                    IdContrato = reader.GetInt32("IdContrato")
                });
            }

            return lista;
        }

        public PagosModels ObtenerPorId(int id)
        {
            PagosModels? pago = null;
            using var connection = GetConnection();
            connection.Open();

            var sql = @"SELECT IdPago, NroPago, FechaPago, Monto, Detalle, IdContrato, Anulado 
                        FROM pagos WHERE IdPago = @id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                pago = new PagosModels
                {
                    IdPago = reader.GetInt32("IdPago"),
                    NroPago = reader.GetInt32("NroPago"),
                    FechaPago = DateOnly.FromDateTime(reader.GetDateTime("FechaPago")),
                    Monto = reader.GetDecimal("Monto"),
                    Detalle = reader.GetString("Detalle"),
                    IdContrato = reader.GetInt32("IdContrato")
                };
            }

            if (pago == null)
                throw new KeyNotFoundException($"No se encontró el pago con Id {id}");

            return pago;
        }

        public IList<PagosModels> ObtenerPagosPorContrato(int idContrato)
        {
            var lista = new List<PagosModels>();
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
                    IdContrato = reader.GetInt32("IdContrato")
                });
            }

            return lista;
        }

        public IList<PagosModels> ObtenerLista(int paginaNro, int tamPag)
        {
            var lista = new List<PagosModels>();
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
                    IdContrato = reader.GetInt32("IdContrato")
                });
            }

            return lista;
        }

        public int ObtenerCantidad()
        {
            int cantidad = 0;
            using var connection = GetConnection();
            connection.Open();

            var sql = @"SELECT COUNT(*) FROM pagos";
            using var command = new MySqlCommand(sql, connection);
            cantidad = Convert.ToInt32(command.ExecuteScalar());

            return cantidad;
        }

        public void AnularPago(int idPago)
        {
            Baja(idPago);
        }
    }
}
