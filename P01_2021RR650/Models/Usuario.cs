using System.ComponentModel.DataAnnotations;

namespace P01_2021RR650.Models
{
    public class Usuario
    {
        [Key]
        public int usuarioId { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string rol { get; set; }
    }
}
