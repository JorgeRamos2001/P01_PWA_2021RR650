using System.ComponentModel.DataAnnotations;

namespace P01_2021RR650.Models
{
    public class Sucursal
    {
        [Key]
        public int sucursalId { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public int cantidadParqueos { get; set; }
        public int usuarioId { get; set; }
    }
}
