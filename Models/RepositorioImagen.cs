using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProyectoInmobiliaria.Models
{
    public class RepositorioImagen : RepositorioBase, IRepositorioImagen
    {
        public RepositorioImagen(IConfiguration configuration) : base(configuration)
        {

        }

        // ALTA
        public int Alta(ImagenModel p)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"INSERT INTO imagen (IdInmueble, UrlImagen) 
                            VALUES (@IdInmueble, @UrlImagen);
                            SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdInmueble", p.IdInmueble);
                    command.Parameters.AddWithValue("@UrlImagen", p.Url ?? string.Empty);

                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.ImagenId = res;
                }
            }
            return res;
        }

        //BAJA
        public int Baja(int id)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"DELETE FROM imagen WHERE IdImagen =@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        //MODIFICACION
        public int Modificacion(ImagenModel p)
        {
            int res = -1;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"UPDATE imagen SET UrlImagen=@UrlImagen WHERE IdImagen=@IdImagen";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdImagen", p.ImagenId);
                    command.Parameters.AddWithValue("@UrlImagen", p.Url ?? string.Empty);
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        // BUSCAR POR INMUEBLE
        public IList<ImagenModel> BuscarPorInmueble(int idInmueble)
        {
            var lista = new List<ImagenModel>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT IdImagen, IdInmueble, UrlImagen 
                            FROM imagen
                            WHERE IdInmueble = @idInmueble";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idInmueble", idInmueble);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var img = new ImagenModel
                            {
                                ImagenId = reader.GetInt32("IdImagen"),
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                Url = reader.GetString("UrlImagen")
                            };
                            lista.Add(img);
                        }
                    }
                }
            }
            return lista;
        }

        // OBTENER POR ID
        public ImagenModel ObtenerPorId(int id)
        {
            ImagenModel? img = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT IdImagen, IdInmueble, UrlImagen 
                            FROM imagen 
                            WHERE IdImagen = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            img = new ImagenModel
                            {
                                ImagenId = reader.GetInt32("IdImagen"),
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                Url = reader.GetString("UrlImagen")
                            };
                        }
                    }
                }
            }
            if (img == null)
                throw new KeyNotFoundException($"No se encontró una imagen con Id={id}");
                
            return img;
        }

        // OBTENER TODOS
        public IList<ImagenModel> ObtenerTodos()
        {
            var lista = new List<ImagenModel>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var sql = @"SELECT IdImagen, IdInmueble, UrlImagen 
                            FROM imagen";
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var img = new ImagenModel
                            {
                                ImagenId = reader.GetInt32("IdImagen"),
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                Url = reader.GetString("UrlImagen")
                            };
                            lista.Add(img);
                        }
                    }
                }
            }
            return lista;
        }

    }

}