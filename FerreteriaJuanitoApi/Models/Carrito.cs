using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FerreteriaJuanitoApi.Models
{
    public class Carrito
    {
        [Key]
        public Guid Id { get; set; }
        public bool Activo { get; set; }
        [ForeignKey(nameof(Cliente))]
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public List<CarritoItem> CarritosItems { get; set; }
        public decimal SubTotal { get; set; }

        [ScaffoldColumn(false)]
        public string MensajeActualizacion {  get; set; }
    }
}
