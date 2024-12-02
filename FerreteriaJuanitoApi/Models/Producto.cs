using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FerreteriaJuanitoApi.Models
{
    public class Producto
    {
        [Key]
        public Guid Id { get; set; }

        public string Nombre { get; set; }
        public string Description { get; set; }
        public decimal PrecioVigente { get; set; }
        public bool Activo { get; set; }

        [ForeignKey(nameof(Categoria))]
        public Guid? CategoriaId { get; set; } // Categoria opcional, por si no se requiere para algunos productos
        public Categoria Categoria { get; set; }
    }
}
