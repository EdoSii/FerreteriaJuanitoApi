using FerreteriaJuanitoApi.Data;
using FerreteriaJuanitoApi.Models;
using FerreteriaJuanitoApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FerreteriaJuanitoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private readonly IProductosService _productosService;
        private readonly ICarritoService _carritoService;
        public readonly DataContext _context;
        public ProductosController(IProductosService productosService, ICarritoService carritoService, DataContext context)
        {
            _productosService = productosService;
            _carritoService = carritoService;
            _context = context;
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("CrearProducto")]
        public IActionResult Create(Producto producto)
        {
            string mensaje = string.Empty;
            //validar que el producto existe
            if (_productosService.Existe(producto))
            {
                _productosService.Create(producto);
                mensaje = "Producto Creado Existosamente";
                Log.Information(mensaje +", Con nombre: "+ producto.Nombre);

            }
            else
            {
                mensaje = "El nombre del producto ya existe; debes ingresar uno diferente";
                Log.Information(mensaje + ", nombre existente: " + producto.Nombre);
            }

            return Ok(mensaje);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("Editar")]
        public IActionResult Update(Guid id, Producto producto) 
        {
            //validar id del producto
            if(id != producto.Id)
            {
                Log.Information($"No se encontro producto con el id: {id} ");
                return NotFound();
            }

            var carritoItems = _context.CarritosItems
                .Include(c => c.Carrito)
                .ThenInclude(c => c.CarritosItems)
                .Where(c => c.ProductoId == id && c.Carrito.Activo == true).ToList();

            var productoDB = _context.Productos
                .FirstOrDefault(p => p.Id == id);

            if (ModelState.IsValid) 
            {
                try
                {
                    if (productoDB.PrecioVigente != producto.PrecioVigente)
                    {
                        foreach (var item in carritoItems)
                        {
                            item.Carrito.SubTotal = item.Carrito.CarritosItems.Sum(s => s.SubTotal);
                            item.Carrito.MensajeActualizacion = "Precio Actualizado";
                        }
                    }
                    productoDB.PrecioVigente = producto.PrecioVigente;
                    productoDB.Description = producto.Description;
                    productoDB.Activo = producto.Activo;
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    //validar que el producto exista
                    if(!_productosService.ProductoExiste(producto.Id))
                    {
                        Log.Information($"No se encontro producto con el id: {id} ");
                        return NotFound();
                    }
                    else
                    {
                        Log.Error(ex, "Ocurrió un error al ejecutar cierto código");
                        throw ex;
                    }
                }
                return RedirectToAction("Index");
            }
            Log.Information($"Se modifico el producto {producto.Nombre} exitosamente");
            return Ok("Se modifico el producto exitosamente");
        }

        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpDelete("Delete")]
        public IActionResult Delete(Guid id)
        {
            _productosService.Delete(id);
            Log.Information($"El producto con id: {id}, fue eliminado existosamente");
            return Ok($"El producto con id: {id}, fue eliminado existosamente");
        }
    }
}
