using FerreteriaJuanitoApi.Data;
using FerreteriaJuanitoApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerreteriaJuanitoApi.Services
{
    public interface ICarritoService
    {
        Task<Carrito> VaciarCarritoAsync(Guid idClienteLogueado);
        Task<bool> HayStockAsync(List<CarritoItem> carritoItems);
    }

    public class CarritoService : ICarritoService
    {
        private readonly DataContext _context;

        public CarritoService(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Carrito> VaciarCarritoAsync(Guid idClienteLogueado)
        {
            var carrito = await _context.Carritos
                .Include(c => c.CarritosItems)
                .FirstOrDefaultAsync(c => c.ClienteId == idClienteLogueado && c.Activo);

            if (carrito == null)
            {
                Log.Error("Carrito no encontrado");
                throw new Exception("Carrito no encontrado.");
            }

            carrito.CarritosItems.Clear();
            await _context.SaveChangesAsync();
            return carrito;
        }

        public async Task<bool> HayStockAsync(List<CarritoItem> carritoItems)
        {
            foreach (var item in carritoItems)
            {
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.Id == item.ProductoId);
                var stockitem = await _context.StockItems
                    .FirstOrDefaultAsync(p => p.Id == item.ProductoId);
                if (producto == null || stockitem.Cantidad < item.Cantidad)
                {
                    return false; // No hay suficiente stock para ese producto
                }
            }
            return true; // Todo está en orden
        }

        public async Task RealizarCompraAsync(Guid carritoId, Guid clienteId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.CarritosItems)
                .ThenInclude(ci => ci.Producto)
                .FirstOrDefaultAsync(c => c.Id == carritoId && c.ClienteId == clienteId && c.Activo);

            if (carrito == null)
            {
                throw new Exception("Carrito no encontrado o no está activo.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Verificamos que haya stock disponible
                    if (!await HayStockAsync(carrito.CarritosItems))
                    {
                        throw new InvalidOperationException("No hay suficiente stock para realizar la compra.");
                    }

                    // Procesamos la compra y actualizamos el stock
                    foreach (var item in carrito.CarritosItems)
                    {
                        var stockItem = await _context.StockItems
                            .FirstOrDefaultAsync(s => s.ProductoId == item.ProductoId);

                        if (stockItem != null)
                        {
                            stockItem.ActualizarStock(item.Cantidad); // Actualizamos el stock
                        }
                    }

                    // Vaciamos el carrito y lo marcamos como inactivo
                    carrito.Activo = false;
                    _context.CarritosItems.RemoveRange(carrito.CarritosItems); // Limpiamos los items del carrito
                    await _context.SaveChangesAsync();

                    // Guardamos la compra
                    var compra = new Compra
                    {
                        ClienteId = clienteId,
                        CarritoId = carritoId,
                        Total = carrito.CarritosItems.Sum(ci => ci.SubTotal),
                        FechaCompra = DateTime.UtcNow
                    };
                    _context.Compras.Add(compra);
                    await _context.SaveChangesAsync();

                    // Finalizamos la transacción
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error(ex, "Error al realizar la compra");
                    throw;
                }
            }
        }

    }
}
