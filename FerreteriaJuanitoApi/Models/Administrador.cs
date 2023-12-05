using System.Text.Json.Serialization;

namespace FerreteriaJuanitoApi.Models
{
    public class Administrador : Usuario
    {
        public override Rol Rol => Rol.Administrador;

        //Constructor de la clase base
        [JsonConstructor]
        public Administrador() : base() { }
    }
}
