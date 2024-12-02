using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FerreteriaJuanitoApi.Models
{
    public class StockItems
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Producto))]
        public Guid ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }

        // Método para actualizar el stock
        public void ActualizarStock(int cantidadVendida)
        {
            if (Cantidad >= cantidadVendida)
            {
                Cantidad -= cantidadVendida;
            }
            else
            {
                throw new InvalidOperationException("No hay suficiente stock.");
            }
        }
    }
}
