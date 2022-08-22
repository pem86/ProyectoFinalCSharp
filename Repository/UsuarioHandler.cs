using ProyectoFinalCSharp.Model;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoFinalCSharp.Repository
{
    public static class UsuarioHandler
    {
        public const string ConnectionString = "Server=WS02\\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True";

        public static bool IniciarSesion(string nombreUsuario, string contrasenia)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText =
                            "SELECT * FROM Usuario " +
                            "WHERE NombreUsuario = @nombreUsuario " +
                            "AND Contraseña = @contrasenia;";
                    sqlCommand.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                    sqlCommand.Parameters.AddWithValue("@contrasenia", contrasenia);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            resultado = true;
                        }
                    }

                    sqlConnection.Close();
                }
            }
            return resultado;
        }

        public static bool CrearUsuario(Usuario usuario)
        {
            bool resultado = false;

            if (TraerUsuario(usuario.NombreUsuario).Id == 0)
            {
                if (usuario.Nombre != string.Empty && usuario.Apellido != string.Empty && usuario.NombreUsuario != string.Empty && 
                    usuario.Contrasenia != string.Empty && usuario.Mail != string.Empty)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                    {
                        string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Usuario] " +
                                             "(Nombre, Apellido, NombreUsuario, Contraseña, Mail) " +
                                             "VALUES (@nombre, @apellido, @nombreUsuario, @contrasenia, @mail);";

                        SqlParameter nombre = new SqlParameter("nombre", SqlDbType.VarChar) { Value = usuario.Nombre };
                        SqlParameter apellido = new SqlParameter("apellido", SqlDbType.VarChar) { Value = usuario.Apellido };
                        SqlParameter nombreUsuario = new SqlParameter("nombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario };
                        SqlParameter contrasenia = new SqlParameter("contrasenia", SqlDbType.VarChar) { Value = usuario.Contrasenia };
                        SqlParameter mail = new SqlParameter("mail", SqlDbType.VarChar) { Value = usuario.Mail };

                        sqlConnection.Open();

                        using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                        {
                            sqlCommand.Parameters.Add(nombre);
                            sqlCommand.Parameters.Add(apellido);
                            sqlCommand.Parameters.Add(nombreUsuario);
                            sqlCommand.Parameters.Add(contrasenia);
                            sqlCommand.Parameters.Add(mail);

                            int numberOfRows = sqlCommand.ExecuteNonQuery();

                            if (numberOfRows > 0)
                            {
                                resultado = true;
                            }

                            sqlConnection.Close();
                        }
                    }
                }
            }

            return resultado;
        }

        public static bool ModificarUsuario(Usuario usuario)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Usuario] " +
                                     "SET Nombre = @nombre, Apellido = @apellido, NombreUsuario = @nombreUsuario, " +
                                     "Contraseña = @contrasenia, Mail = @mail WHERE Id = @id;";

                SqlParameter id = new SqlParameter("id", SqlDbType.BigInt) { Value = usuario.Id };
                SqlParameter nombre = new SqlParameter("nombre", SqlDbType.VarChar) { Value = usuario.Nombre };
                SqlParameter apellido = new SqlParameter("apellido", SqlDbType.VarChar) { Value = usuario.Apellido };
                SqlParameter nombreUsuario = new SqlParameter("nombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario };
                SqlParameter contrasenia = new SqlParameter("contrasenia", SqlDbType.VarChar) { Value = usuario.Contrasenia };
                SqlParameter mail = new SqlParameter("mail", SqlDbType.VarChar) { Value = usuario.Mail };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(id);
                    sqlCommand.Parameters.Add(nombre);
                    sqlCommand.Parameters.Add(apellido);
                    sqlCommand.Parameters.Add(nombreUsuario);
                    sqlCommand.Parameters.Add(contrasenia);
                    sqlCommand.Parameters.Add(mail);

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
                
        public static Usuario TraerUsuario(string nombreUsuario)
        {
            Usuario usuario = new Usuario();
            
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "SELECT * FROM Usuario WHERE NombreUsuario = @nombreUsuario;";
                    sqlCommand.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                    
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            
                            usuario.Id = Convert.ToInt32(dataReader["Id"]);
                            usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                            usuario.Nombre = dataReader["Nombre"].ToString();
                            usuario.Apellido = dataReader["Apellido"].ToString();
                            usuario.Contrasenia = "xxx";
                            usuario.Mail = dataReader["Mail"].ToString();
                        }
                    }
                    sqlConnection.Close();
                }

                return usuario;
            }
        }

        public static bool EliminarUsuario(int id)
        {
            bool resultado = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[Usuario] WHERE Id = @idParametro;";

                SqlParameter idParametro = new SqlParameter("idParametro", SqlDbType.BigInt) { Value = id };

                sqlConnection.Open();

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
