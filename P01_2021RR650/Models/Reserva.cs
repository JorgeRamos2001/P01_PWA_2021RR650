using System.ComponentModel.DataAnnotations;

namespace P01_2021RR650.Models
{
    public class Reserva
    {
        [Key]
        public int reservaId { get; set; }
        public DateTime fechaReserva { get; set; }
        public int cantidadHoras { get; set; }
        public int usuarioId { get; set; }
        public int parqueoId { get; set; }
    }
}
