using Microsoft.AspNetCore.Mvc;
using ProyectoFinalCSharp.Controllers.DTOs;
using ProyectoFinalCSharp.Model;
using ProyectoFinalCSharp.Repository;

namespace ProyectoFinalCSharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosVendidosController : ControllerBase
    {
        [HttpGet(Name = "TraerProductosVendidos")]
        public List<GetProductoVendido> TraerProductosVendidos(int idUsuario)
        {
            List<ProductoVendido> productosVendidos = ProductosVendidosHandler.TraerProductosVendidos(idUsuario);

            List<GetProductoVendido> resultados = new List<GetProductoVendido>();

            foreach(ProductoVendido productoVendido in productosVendidos)
            {
                GetProductoVendido resultado = new GetProductoVendido
                {
                    NombreUsuario = productoVendido.NombreUsuario,
                    Producto = productoVendido.Producto,
                    Stock = productoVendido.Stock,
                    Comentarios = productoVendido.Comentarios
                };

                resultados.Add(resultado);
            }
            return resultados;
        }
    }
}
