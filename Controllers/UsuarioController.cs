using Microsoft.AspNetCore.Mvc;
using ProyectoFinalCSharp.Controllers.DTOs;
using ProyectoFinalCSharp.Model;
using ProyectoFinalCSharp.Repository;

namespace ProyectoFinalCSharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        [HttpPost(Name = "CrearUsuario")]
        public bool CrearUsuario([FromBody] PostUsuario usuario)
        {
            return UsuarioHandler.CrearUsuario(new Usuario
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                NombreUsuario = usuario.NombreUsuario,
                Contrasenia = usuario.Contrasenia,
                Mail = usuario.Mail
            });
        }

        [HttpPut(Name = "ModificarUsuario")]
        public bool ModificarUsuario([FromBody] PutUsuario usuario)
        {
            return UsuarioHandler.ModificarUsuario(new Usuario
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                NombreUsuario = usuario.NombreUsuario,
                Contrasenia = usuario.Contrasenia,
                Mail = usuario.Mail
            });
        }

        [HttpGet(Name = "TraerUsuario")]
        public GetUsuario TraerUsuario(string NombreUsuario)
        {
            Usuario usuario = UsuarioHandler.TraerUsuario(NombreUsuario);

            GetUsuario resultado = new GetUsuario();

            resultado.Id = usuario.Id;
            resultado.Nombre = usuario.Nombre;
            resultado.Apellido = usuario.Apellido;
            resultado.NombreUsuario = usuario.NombreUsuario;
            resultado.Mail = usuario.Mail;

            return resultado;
        }

        [HttpDelete(Name = "EliminarUsuario")]
        public bool EliminarUsuario([FromBody] int Id)
        {
            return UsuarioHandler.EliminarUsuario(Id);
        }
    }
}
