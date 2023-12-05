using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FerreteriaJuanitoApi.Models
{
    public class Cliente : Usuario
    {
        public string Rut {  get; set; }
        public List<Compra> Compras { get; set; }
        public List<Carrito> Carritos { get; set; }
        public override Rol Rol => Rol.Cliente;

        //Constructor de la clase base
        [JsonConstructor]
        public Cliente() : base()
        {
            Rut = string.Empty;
            Compras = new List<Compra>();
            Carritos = new List<Carrito>();
        }

    }
}
