using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FerreteriaJuanitoApi.Models
{
    public class CarritoItem
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(Carrito))]
        public Guid CarritoId { get; set; }
        public Carrito Carrito { get; set; }
        [ForeignKey(nameof(Producto))]
        public Guid ProductoId { get; set; }
        public Producto Producto { get; set; }
        public decimal ValorUnitario { get; set; }
        public int Cantidad { get; set; }  
        public decimal SubTotal { get; set; }
    }
}
