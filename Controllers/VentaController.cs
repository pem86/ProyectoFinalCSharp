using Microsoft.AspNetCore.Mvc;
using ProyectoFinalCSharp.Controllers.DTOs;
using ProyectoFinalCSharp.Model;
using ProyectoFinalCSharp.Repository;

namespace ProyectoFinalCSharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VentaController : Controller
    {
        [HttpPost(Name = "CargarVenta")]
        public void CargarVenta([FromBody] List<PostVenta> productosPost, int idUsuario, string comentarios)
        {
            List<Producto> productosVenta = new List<Producto>();

            foreach(PostVenta productoPost in productosPost)
            {
                Producto productoVenta = new Producto();

                productoVenta.Id = productoPost.IdProducto;
                productoVenta.Stock = productoPost.Stock;

                productosVenta.Add(productoVenta);                               
            }
            VentaHandler.CargarVenta(productosVenta, idUsuario, comentarios);
        }

        [HttpGet(Name = "TraerVentas")]
        public List<GetVenta> TraerVentas()
        {
            List<Venta> ventas = VentaHandler.TraerVentas();

            List<GetVenta> resultados = new List<GetVenta>();

            foreach(Venta venta in ventas)
            {
                GetVenta resultado = new GetVenta();

                resultado.NombreUsuario = venta.NombreUsuario;
                resultado.Producto = venta.Producto;
                resultado.Stock = venta.Stock;
                resultado.Comentarios = venta.Comentarios;

                resultados.Add(resultado);
            }
            return resultados;
        }
        
        [HttpDelete(Name = "EliminarVenta")]
        public void EliminarVenta([FromBody] int id)
        {
            VentaHandler.EliminarVenta(id);
        }
    }
}
