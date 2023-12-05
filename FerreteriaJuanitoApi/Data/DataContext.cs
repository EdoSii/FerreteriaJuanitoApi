using FerreteriaJuanitoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System.Reflection;

namespace FerreteriaJuanitoApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Usuario> Users { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoItem> CarritosItems { get; set;}
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set;}
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Administrador> Administradores { get; set;}
        public DbSet<StockItems> StockItems { get; set; }
        public DbSet<Login> Login { get; set; }

    }
}
