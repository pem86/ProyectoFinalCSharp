using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalCSharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NombreController : ControllerBase
    {
        [HttpGet(Name ="TraerNombre")]
        public string TraerNombre()
        {
            return "ProyectoFinalCSharp";
        }

    }
}
