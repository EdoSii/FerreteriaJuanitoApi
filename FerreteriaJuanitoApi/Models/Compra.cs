using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FerreteriaJuanitoApi.Models
{
    public class Compra
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Total { get; set; }

        [ForeignKey(nameof(Cliente))]
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [ForeignKey(nameof(Carrito))]
        public Guid CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        [Display(Name = "Fecha de Compra")]
        public DateTime FechaCompra { get; set; } = DateTime.UtcNow; // Establecer valor por defecto
    }
}
