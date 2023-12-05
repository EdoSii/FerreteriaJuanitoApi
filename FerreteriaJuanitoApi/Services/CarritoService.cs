using FerreteriaJuanitoApi.Data;
using FerreteriaJuanitoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FerreteriaJuanitoApi.Services
{
    public interface ICarritoService
    {
        void EliminarItem(Guid id);
        Carrito VaciarCarrito(Guid idClienteLogueado);
    }
    public class CarritoService : ICarritoService
    {
        public readonly DataContext _context;
        public CarritoService(DataContext context) 
        {
            _context = context;
        }
        public Carrito VaciarCarrito(Guid idClienteLogueado) 
        {
            Carrito carrito = new Carrito();
            try
            {
                carrito = _context.Carritos
                    .Include(c => c.CarritosItems)
                    .ThenInclude(m => m.Producto)
                    .Include(c => c.Cliente)
                    .FirstOrDefault(m => m.ClienteId == idClienteLogueado && m.Activo == true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Hubo un error al tratar de vaciar el carrito");
                throw ;
            }

            //validar que carrito no sea null
            if (carrito == null)
            {
                Log.Error("Carrito no encontrado");
                throw new Exception();
            }
            return carrito;
        }

        public void EliminarItem(Guid id)
        {
            CarritoItem carritoItems = new CarritoItem();
            try
            {
                carritoItems = _context.CarritosItems
                    .Include(m => m.Carrito)
                    .ThenInclude(m => m.CarritosItems)
                    .Include(m => m.Producto)
                    .FirstOrDefault(c => c.Id == id);

                carritoItems.Carrito.SubTotal = carritoItems.Carrito.CarritosItems.Sum(s => s.SubTotal) - carritoItems.SubTotal;
                _context.CarritosItems.Remove(carritoItems);
                _context.SaveChanges();
            }
            catch (Exception ex) 
            {
                Log.Error(ex, "Hubo un error al tratar de eliminar producto");
                throw;
            }
        }
    }
}
