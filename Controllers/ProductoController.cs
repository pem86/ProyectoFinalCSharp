using Microsoft.AspNetCore.Mvc;
using ProyectoFinalCSharp.Controllers.DTOs;
using ProyectoFinalCSharp.Model;
using ProyectoFinalCSharp.Repository;

namespace ProyectoFinalCSharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoController : ControllerBase
    {
        [HttpPost(Name = "CrearProducto")]
        public bool CrearProducto([FromBody] PostProducto producto)
        {
            return ProductoHandler.CrearProducto(new Producto
            {
                Descripcion = producto.Descripcion,
                Costo = producto.Costo,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock
            });
        }

        [HttpPut(Name = "ModificarProducto")]
        public bool ModificarProducto([FromBody] PutProducto producto)
        {
            return ProductoHandler.ModificarProducto(new Producto
            {
                Id = producto.Id,
                Descripcion = producto.Descripcion,
                Costo = producto.Costo,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
                IdUsuario = producto.IdUsuario
            });
        }
        
        [HttpGet(Name = "TraerProductos")]
        public List<GetProducto> TraerProductos()
        {
            List<Producto> productos = ProductoHandler.TraerProductos();

            List<GetProducto> resultados = new List<GetProducto>();

            foreach(Producto producto in productos)
            {
                GetProducto resultado = new GetProducto
                {
                    Id = producto.Id,
                    Descripcion = producto.Descripcion,
                    Costo = producto.Costo,
                    PrecioVenta = producto.PrecioVenta,
                    Stock = producto.Stock,
                    IdUsuario = producto.IdUsuario
                };

                resultados.Add(resultado);
            }
            return resultados;
        }
        
        [HttpDelete(Name = "EliminarProducto")]
        public bool EliminarProducto([FromBody] int Id)
        {
            return ProductoHandler.EliminarProducto(Id);
        }
    }
}