using FerreteriaJuanitoApi.Data;
using FerreteriaJuanitoApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FerreteriaJuanitoApi.Services
{
    public interface IProductosService
    {
        Producto Create(Producto producto);
        Producto Edit(Guid id);
        void Delete(Guid id);
        bool Existe(Producto producto);
        bool ProductoExiste(Guid id);
    }
    public class ProductosService : IProductosService
    {
        public readonly DataContext _context;
        public ProductosService(DataContext dbContext)
        {
            _context = dbContext;
        }
        public Producto Create(Producto producto)
        {
            try
            {
                producto.Id = Guid.NewGuid();
                _context.Add(producto);
                _context.SaveChanges();
                
            }
            catch (Exception ex) 
            {
                Log.Error(ex, "Hubo un error al tratar de crear producto");
                throw;
            }
            return producto;
        }

        public Producto Edit(Guid id) 
        {
            Producto producto = new Producto();
            try
            {
                producto = _context.Productos.FirstOrDefault(p => p.Id == id);
            }
            catch (Exception ex) 
            {
                Log.Error(ex, "Hubo un error al tratar de editar el producto");
                throw;
            }

            //validar que producto no sea null
            if(producto == null)
            {
                Log.Error($"Producto existe {id}");
                throw new Exception();
            }
            return producto;

        }
        public void Delete(Guid id) 
        {
            Producto producto = new Producto();
            try
            {
                producto = _context.Productos.Find(id);
                _context.Productos.Remove(producto);
                _context.SaveChanges(true);

            }
            catch(Exception ex) 
            {
                Log.Error(ex, "Hubo un error al tratar de eliminar producto");
                throw;
            }
        }
        public bool Existe(Producto producto)
        {
            bool existe = false;
            if(_context.Productos.Any(p => p.Nombre == producto.Nombre))
                existe = true;
            return existe;
        }
        public bool ProductoExiste(Guid id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
