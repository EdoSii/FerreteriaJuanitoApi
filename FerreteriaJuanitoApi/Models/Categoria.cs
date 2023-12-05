using System.ComponentModel.DataAnnotations;

namespace FerreteriaJuanitoApi.Models
{
    public class Categoria
    {
        [Key]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {  get; set; }
        public List<Producto> Productos { get; set; }
    }
}
