using FerreteriaJuanitoApi.Data;
using FerreteriaJuanitoApi.Models;
using FerreteriaJuanitoApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Transactions;

namespace FerreteriaJuanitoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;
        private readonly DataContext _context;

        public CarritoController(ICarritoService carritoService, DataContext context)
        {
            _carritoService = carritoService ?? throw new ArgumentNullException(nameof(carritoService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Endpoint para agregar productos al carrito
        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpPost("AgregarProducto")]
        public async Task<IActionResult> AgregarProductoAlCarrito(Guid productoId, int cantidad)
        {
            if (cantidad <= 0)
                return BadRequest("La cantidad debe ser mayor a 0");

            var idClienteLogueado = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verificamos si el cliente tiene un carrito activo
            var carrito = await _context.Carritos
                .Include(c => c.CarritosItems)
                .FirstOrDefaultAsync(c => c.ClienteId == idClienteLogueado && c.Activo);

            if (carrito == null)
                return NotFound("No se encontró un carrito activo para el cliente.");

            // Obtenemos el producto y el stock de manera eficiente
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == productoId);

            var stockItem = await _context.StockItems
                .FirstOrDefaultAsync(s => s.ProductoId == productoId);

            if (producto == null)
                return NotFound("Producto no encontrado.");

            if (stockItem == null || stockItem.Cantidad < cantidad)
                return BadRequest("No hay suficiente stock para agregar el producto.");

            // Verificamos si el producto ya está en el carrito
            var carritoItemExistente = carrito.CarritosItems
                .FirstOrDefault(c => c.ProductoId == productoId);

            if (carritoItemExistente != null)
            {
                // Si el producto ya está en el carrito, actualizamos la cantidad
                carritoItemExistente.Cantidad += cantidad;  // Esto actualizará el subtotal automáticamente
            }
            else
            {
                // Si no existe, agregamos un nuevo item al carrito
                var nuevoCarritoItem = new CarritoItem
                {
                    ProductoId = productoId,
                    Cantidad = cantidad
                    // No es necesario asignar SubTotal, ya que se calcula automáticamente
                };

                carrito.CarritosItems.Add(nuevoCarritoItem);
            }

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Producto agregado al carrito.");
        }

        // Endpoint para realizar la compra
        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpPost("RealizarCompra")]
        public async Task<IActionResult> RealizarCompra(Guid carritoId)
        {
            if (carritoId == Guid.Empty)
                return BadRequest("El ID del carrito no puede estar vacío.");

            var idClienteLogueado = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var carrito = await _context.Carritos
                .Include(c => c.CarritosItems)
                .ThenInclude(ci => ci.Producto)
                .FirstOrDefaultAsync(c => c.Id == carritoId && c.ClienteId == idClienteLogueado && c.Activo);

            if (carrito == null)
                return NotFound("Carrito no encontrado o no es válido para este cliente.");

            // Iniciar una transacción para asegurar consistencia
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Verificamos que haya stock suficiente para todos los items en el carrito
                    if (!await _carritoService.HayStockAsync(carrito.CarritosItems))
                    {
                        return BadRequest("No hay suficiente stock para realizar la compra.");
                    }

                    // Descontamos el stock de los productos
                    foreach (var item in carrito.CarritosItems)
                    {
                        var stockItem = await _context.StockItems
                            .FirstOrDefaultAsync(s => s.ProductoId == item.ProductoId);

                        if (stockItem != null)
                        {
                            // Actualizamos el stock de acuerdo con la cantidad comprada
                            stockItem.Cantidad -= item.Cantidad;
                        }
                    }

                    // Vaciar el carrito después de realizar la compra
                    await _carritoService.VaciarCarritoAsync(idClienteLogueado);

                    // Guardamos los cambios en la base de datos
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok("Compra realizada con éxito.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error(ex, "Error al realizar la compra.");
                    return StatusCode(500, "Ocurrió un error al procesar la compra.");
                }
            }
        }
    }
}
