using ProyectoFinalCSharp.Model;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoFinalCSharp.Repository
{
    public static class ProductosVendidosHandler
    {
        public const string ConnectionString = "Server=WS02\\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True";
        
        public static List<ProductoVendido> TraerProductosVendidos(int id)
        {
            List<ProductoVendido> productosVendidos = new List<ProductoVendido>();
            
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string querySelect = "SELECT u.NombreUsuario, p.Descripciones AS Producto, v.Stock, o.Comentarios " +
                    "FROM[SistemaGestion].[dbo].[Producto] AS p " +
                    "INNER JOIN[SistemaGestion].[dbo].[Usuario] AS u ON p.IdUsuario = u.Id " +
                    "INNER JOIN[SistemaGestion].[dbo].[ProductoVendido] AS v ON v.IdProducto = p.Id " +
                    "INNER JOIN[SistemaGestion].[dbo].[Venta] AS o ON v.IdVenta = o.Id " +
                    "WHERE p.IdUsuario = @idUsuario;";

                SqlParameter idUsuario = new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = id };

                using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection))
                {
                    sqlCommand.Parameters.Add(idUsuario);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();

                                productoVendido.NombreUsuario = dataReader["NombreUsuario"].ToString();
                                productoVendido.Producto = dataReader["Producto"].ToString();
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.Comentarios = dataReader["Comentarios"].ToString();

                                productosVendidos.Add(productoVendido);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return productosVendidos;
        }
    }
}
