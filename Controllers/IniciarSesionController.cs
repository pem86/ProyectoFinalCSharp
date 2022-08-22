using Microsoft.AspNetCore.Mvc;
using ProyectoFinalCSharp.Repository;

namespace ProyectoFinalCSharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IniciarSesionController : ControllerBase
    {
        [HttpGet(Name = "IniciarSesion")]
        public bool IniciarSesion(string NombreUsuario, string Contrasenia)
        {
            return UsuarioHandler.IniciarSesion(NombreUsuario, Contrasenia);
        }
    }
}