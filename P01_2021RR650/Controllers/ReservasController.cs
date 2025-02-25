using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2021RR650.Models;

namespace P01_2021RR650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly AplicacionDBContext _contexto;

        public ReservasController(AplicacionDBContext contexto)
        {
            _contexto = contexto;
        }

        [HttpPost]
        [Route("/ReservarParqueo")]
        public IActionResult ReservarParqueo([FromBody]Reserva reserva)
        {
            try
            {
                Usuario? usuario = (from e in _contexto.Usuarios where e.usuarioId == reserva.usuarioId select e).FirstOrDefault();

                if (usuario == null)
                {
                    return NotFound($"Usuario con id: {reserva.usuarioId} no encontrado.");
                }

                Parqueo? parqueo = (from p in _contexto.Parqueos where p.parqueoId == reserva.parqueoId select p).FirstOrDefault();

                if (parqueo == null)
                {
                    return NotFound($"Parqueo con el id: {reserva.parqueoId} no encontrado.");
                }

                if (!parqueo.estado.Equals("DISPONIBLE"))
                {
                    return BadRequest($"Parqueo no disponible.");
                }

                _contexto.Reservas.Add(reserva);
                _contexto.SaveChanges();
                return Ok(parqueo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        [Route("/ReservasPorUsuario")]
        public IActionResult ReservasPorUsuario(int usuarioId)
        {
            Usuario? usuario = (from e in _contexto.Usuarios where e.usuarioId == usuarioId select e).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound($"Usuario con id: {usuarioId} no encontrado.");
            }

            DateTime fechaActual = DateTime.Now;

            var reservasActivas = (from r in _contexto.Reservas
                                   where r.usuarioId == usuarioId &&
                                         r.fechaReserva <= fechaActual &&
                                         r.fechaReserva.AddHours(r.cantidadHoras) > fechaActual
                                   select new
                                   {
                                       r.reservaId,
                                       r.fechaReserva,
                                       r.cantidadHoras
                                   }).ToList();

            var usuarioConReservas = new
            {
                usuario.nombre,
                usuario.correo,
                usuario.telefono,
                ReservasActivas = reservasActivas
            };

            return Ok(usuarioConReservas);
        }

        [HttpGet]
        [Route("/EspaciosReservadosPorDia")]
        public IActionResult EspaciosReservadosPorDia()
        {
            var reservasPorDia = (from r in _contexto.Reservas
                                  join p in _contexto.Parqueos on r.parqueoId equals p.parqueoId
                                  join s in _contexto.Sucursales on p.sucursalId equals s.sucursalId
                                  group new { r, p, s } by r.fechaReserva.Date into reservasAgrupadas
                                  select new
                                  {
                                      FechaReserva = reservasAgrupadas.Key,
                                      Sucursales = reservasAgrupadas.Select(ra => new
                                      {
                                          ra.s.nombre,
                                          ra.s.direccion,
                                          ra.s.telefono,
                                          Parqueos = reservasAgrupadas.Select(r => new
                                          {
                                              r.p.ubicacion,
                                              r.p.parqueoId
                                          }).ToList()
                                      }).ToList()
                                  }).ToList();

            return Ok(reservasPorDia);
        }

        [HttpGet]
        [Route("/EspaciosReservadosEntreFechas")]
        public IActionResult EspaciosReservadosEntreFechas(int sucursalId, DateTime fechaInicio, DateTime fechaFin)
        {
            var reservasEntreFechas = (from r in _contexto.Reservas
                                       join p in _contexto.Parqueos on r.parqueoId equals p.parqueoId
                                       where p.sucursalId == sucursalId &&
                                             r.fechaReserva >= fechaInicio &&
                                             r.fechaReserva <= fechaFin
                                       select new
                                       {
                                           r.reservaId,
                                           r.fechaReserva,
                                           r.cantidadHoras,
                                           Parqueo = new
                                           {
                                               p.parqueoId,
                                               p.ubicacion
                                           }
                                       }).ToList();

            return Ok(reservasEntreFechas);
        }
    }
}
