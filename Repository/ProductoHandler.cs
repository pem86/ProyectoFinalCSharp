using ProyectoFinalCSharp.Model;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoFinalCSharp.Repository
{
    public static class ProductoHandler
    {
        public const string ConnectionString = "Server=WS02\\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True";

        public static bool CrearProducto(Producto producto)
        {
            bool resultado = false;

            if (producto.Descripcion != string.Empty && producto.Costo != 0 && producto.PrecioVenta != 0 && producto.Stock != 0)
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Producto] " +
                     "(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " +
                     "VALUES (@descripcion, @costo, @precioVenta, @stock, 1);";

                    SqlParameter descripcion = new SqlParameter("descripcion", SqlDbType.VarChar) { Value = producto.Descripcion };
                    SqlParameter costo = new SqlParameter("costo", SqlDbType.Money) { Value = producto.Costo };
                    SqlParameter precioVenta = new SqlParameter("precioVenta", SqlDbType.Money) { Value = producto.PrecioVenta };
                    SqlParameter stock = new SqlParameter("stock", SqlDbType.Int) { Value = producto.Stock };

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(descripcion);
                        sqlCommand.Parameters.Add(costo);
                        sqlCommand.Parameters.Add(precioVenta);
                        sqlCommand.Parameters.Add(stock);

                        int numberOfRows = sqlCommand.ExecuteNonQuery();

                        if (numberOfRows > 0)
                        {
                            resultado = true;
                        }
                        sqlConnection.Close();
                    }
                }
            }
            return resultado;
        }

        public static bool ModificarProducto(Producto producto)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Producto] " +
                     "SET Descripciones = @descripcion, Costo = @costo, PrecioVenta = @precioVenta, " +
                     "Stock = @stock, IdUsuario = @idUsuario WHERE Id = @id;";

                SqlParameter id = new SqlParameter("id", SqlDbType.BigInt) { Value = producto.Id };
                SqlParameter descripcion = new SqlParameter("descripcion", SqlDbType.VarChar) { Value = producto.Descripcion };
                SqlParameter costo = new SqlParameter("costo", SqlDbType.Money) { Value = producto.Costo };
                SqlParameter precioVenta = new SqlParameter("precioVenta", SqlDbType.Money) { Value = producto.PrecioVenta };
                SqlParameter stock = new SqlParameter("stock", SqlDbType.Int) { Value = producto.Stock };
                SqlParameter idUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = producto.IdUsuario };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(id);
                    sqlCommand.Parameters.Add(descripcion);
                    sqlCommand.Parameters.Add(costo);
                    sqlCommand.Parameters.Add(precioVenta);
                    sqlCommand.Parameters.Add(stock);
                    sqlCommand.Parameters.Add(idUsuario);

                    int numberOfRows = sqlCommand.ExecuteNonQuery();

                    if (numberOfRows > 0)
                    {
                        resultado = true;
                    }
                    sqlConnection.Close();
                }
            }
            return resultado;
        }
        public static List<Producto> TraerProductos()
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "SELECT * FROM Producto;";

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Producto producto = new Producto();

                                producto.Id = Convert.ToInt32(dataReader["Id"]);
                                producto.Descripcion = dataReader["Descripciones"].ToString();
                                producto.Costo = Convert.ToDouble(dataReader["Costo"]);
                                producto.PrecioVenta = Convert.ToDouble(dataReader["PrecioVenta"]);
                                producto.Stock = Convert.ToInt32(dataReader["Stock"]);
                                producto.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);

                                productos.Add(producto);
                            }
                        }
                    }

                    sqlConnection.Close();
                }
            }
            return productos;
        }

        public static bool EliminarProducto(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[ProductoVendido] WHERE IdProducto = @idParametro;";

                SqlParameter idParametro = new SqlParameter("idParametro", SqlDbType.BigInt) { Value = id };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(idParametro);

                    sqlCommand.ExecuteNonQuery();

                    sqlCommand.Parameters.Clear();
                }
                queryDelete = "DELETE FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @idParametro;";

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(idParametro);

                    int numberOfRows = sqlCommand.ExecuteNonQuery();

                    if (numberOfRows > 0)
                    {
                        resultado = true;
                    }

                    sqlConnection.Close();
                }
            }
            return resultado;
        }
    }
}