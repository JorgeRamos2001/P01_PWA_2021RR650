using System.ComponentModel.DataAnnotations;

namespace P01_2021RR650.Models
{
    public class Parqueo
    {
        [Key]
        public int parqueoId { get; set; }
        public string ubicacion { get; set; }
        public decimal costo { get; set; }
        public string estado { get; set; }
        public int sucursalId { get; set; }
    }
}
