using ProyectoFinalCSharp.Model;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoFinalCSharp.Repository
{
    public static class VentaHandler
    {
        public const string ConnectionString = "Server=WS02\\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True";

        public static void CargarVenta(List<Producto> productos, int idUsuario, string comentarios)
        {
            int idVenta;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Venta] (Comentarios) VALUES (@comentariosParametro); SELECT SCOPE_IDENTITY();";

                SqlParameter comentariosParametro = new SqlParameter("comentariosParametro", SqlDbType.VarChar) { Value = comentarios };

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(comentariosParametro);
                    idVenta = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    sqlCommand.Parameters.Clear();
                }

                foreach(Producto producto in productos)
                {
                    queryInsert = "INSERT INTO [SistemaGestion].[dbo].[ProductoVendido] " +
                                  "(Stock, IdProducto, IdVenta) " +
                                  "VALUES (@stockParam, @idProductoParam, @idVentaParam);";

                    SqlParameter stockParam = new SqlParameter("stockParam", SqlDbType.Int) { Value = producto.Stock };
                    SqlParameter idProductoParam = new SqlParameter("idProductoParam", SqlDbType.BigInt) { Value = producto.Id};
                    SqlParameter idVentaParam = new SqlParameter("idVentaParam", SqlDbType.BigInt) { Value = idVenta };

                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(stockParam);
                        sqlCommand.Parameters.Add(idProductoParam);
                        sqlCommand.Parameters.Add(idVentaParam);

                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Parameters.Clear();
                    }

                    string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Producto] SET Stock = Stock - @cantidad WHERE Id = @idProducto;";

                    SqlParameter cantidad = new SqlParameter("cantidad", SqlDbType.Int) { Value = producto.Stock };
                    SqlParameter idProducto = new SqlParameter("idProducto", SqlDbType.BigInt) { Value = producto.Id };

                    using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(cantidad);
                        sqlCommand.Parameters.Add(idProducto);
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Parameters.Clear();
                    }
                }
                sqlConnection.Close();
            }
        }

        public static void EliminarVenta(int idVenta)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                List<Producto> productos = new List<Producto>();

                string querySelect = "SELECT * FROM [SistemaGestion].[dbo].[ProductoVendido] WHERE IdVenta = @idVentaParam;";

                SqlParameter idVentaParam = new SqlParameter("idVentaParam", SqlDbType.VarChar) { Value = idVenta };

                using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection))
                {
                    sqlCommand.Parameters.Add(idVentaParam);

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Producto producto = new Producto();

                                producto.Id = Convert.ToInt32(dataReader["IdProducto"]);
                                producto.Stock = Convert.ToInt32(dataReader["Stock"]);

                                productos.Add(producto);
                            }
                        }
                    }
                    sqlCommand.Parameters.Clear();
                }

                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[ProductoVendido] WHERE IdVenta = @idVentaParam";

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(idVentaParam);
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                }

                foreach(Producto producto in productos)
                {
                    string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Producto] SET Stock = Stock + @cantidad WHERE Id = @idProducto;";

                    SqlParameter cantidad = new SqlParameter("cantidad", SqlDbType.Int) { Value = producto.Stock };
                    SqlParameter idProducto = new SqlParameter("idProducto", SqlDbType.BigInt) { Value = producto.Id };

                    using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(cantidad);
                        sqlCommand.Parameters.Add(idProducto);
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Parameters.Clear();
                    }
                }
                queryDelete = "DELETE FROM [SistemaGestion].[dbo].[Venta] WHERE Id = @idVentaParam";

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(idVentaParam);
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                }
                sqlConnection.Close();
            }
        }

        public static List<Venta> TraerVentas()
        {
            List<Venta> ventas = new List<Venta>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                string querySelect = "SELECT u.NombreUsuario, p.Descripciones AS Producto, v.Stock, o.Comentarios " +
                                     "FROM[SistemaGestion].[dbo].[Producto] AS p " +
                                     "INNER JOIN[SistemaGestion].[dbo].[Usuario] AS u ON p.IdUsuario = u.Id " +
                                     "INNER JOIN[SistemaGestion].[dbo].[ProductoVendido] AS v ON v.IdProducto = p.Id " +
                                     "INNER JOIN[SistemaGestion].[dbo].[Venta] AS o ON v.IdVenta = o.Id;";

                using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection))
                {
                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Venta venta = new Venta();

                                venta.NombreUsuario = dataReader["NombreUsuario"].ToString();
                                venta.Producto = dataReader["Producto"].ToString();
                                venta.Stock = Convert.ToInt32(dataReader["Stock"]);
                                venta.Comentarios = dataReader["Comentarios"].ToString();

                                ventas.Add(venta);
                            }
                        }
                    }
                }
                sqlConnection.Close();
            }
            return ventas;
        }
    }
}
