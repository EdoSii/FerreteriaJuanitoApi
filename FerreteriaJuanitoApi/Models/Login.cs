using System.ComponentModel.DataAnnotations;

namespace FerreteriaJuanitoApi.Models
{
    public class Login
    {
        [Key]
        public string Email { get; set; }
        public string Pwd { get; set; }
    }
}
