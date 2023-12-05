using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FerreteriaJuanitoApi.Models
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion {  get; set; }
        public string Genero { get; set; }
        public string Pwd { get; set; }
        public DateTime MemberSince { get; set; }
        [JsonConstructor]
        protected Usuario()
        {
            Id = Guid.NewGuid();
            // Inicializa otras propiedades si es necesario...
        }
        public virtual Rol Rol { get; set; }

    }
}
