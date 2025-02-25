using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2021RR650.Models;

namespace P01_2021RR650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParqueosController : ControllerBase
    {
        private readonly AplicacionDBContext _contexto;

        public ParqueosController(AplicacionDBContext contexto)
        {
            _contexto = contexto;
        }

        [HttpPost]
        [Route("/AgregarParqueo")]
        public IActionResult AgregarParqueo([FromBody]Parqueo parqueo)
        {
            try
            {
                Sucursal? sucursal = (from s in _contexto.Sucursales where s.sucursalId == parqueo.sucursalId select s).FirstOrDefault();

                if (sucursal == null)
                {
                    return NotFound($"Sucursal con el id: {parqueo.sucursalId} no fue encontrada.");
                }

                _contexto.Parqueos.Add(parqueo);
                _contexto.SaveChanges();
                return Ok(parqueo);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/ParqueosDisponibles")]
        public IActionResult ParqueosDisponibles(int sucursalId)
        {
            Sucursal? sucursal = (from s in _contexto.Sucursales where s.sucursalId == sucursalId select s).FirstOrDefault();

            if (sucursal == null)
            {
                return NotFound($"Sucursal con el id: {sucursalId} no fue encontrada.");
            }

            var parqueosDisponibles = (from p in _contexto.Parqueos
                                       where p.sucursalId == sucursalId && p.estado == "DISPONIBLE"
                                       select new
                                       {
                                           p.parqueoId,
                                           p.ubicacion,
                                           p.costo,
                                           p.estado
                                       }).ToList();

            var sucursalConParqueos = new
            {
                sucursal.nombre,
                sucursal.direccion,
                sucursal.telefono,
                ParqueosDisponibles = parqueosDisponibles
            };

            return Ok(sucursalConParqueos);
        }

        [HttpPut]
        [Route("/ModificarParqueo")]
        public IActionResult ModificarParqueo(int parqueoId, [FromBody] Parqueo parqueoModificado)
        {
            Parqueo? parqueo = (from p in _contexto.Parqueos where p.parqueoId == parqueoId select p).FirstOrDefault();

            if (parqueo == null)
            {
                return NotFound($"Parqueo con el id: {parqueoId} no encontrado.");
            }

            parqueo.ubicacion = parqueoModificado.ubicacion;
            parqueo.costo = parqueoModificado.costo;
            parqueo.estado = parqueoModificado.estado;

            _contexto.Parqueos.Entry(parqueo).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(parqueo);
        }

        [HttpDelete]
        [Route("/EliminarParqueo")]
        public IActionResult EliminarParqueo(int parqueoId)
        {
            Parqueo? parqueo = (from p in _contexto.Parqueos where p.parqueoId == parqueoId select p).FirstOrDefault();

            if (parqueo == null)
            {
                return NotFound($"Parqueo con el id: {parqueoId} no encontrado.");
            }

            _contexto.Parqueos.Attach(parqueo);
            _contexto.Parqueos.Remove(parqueo);
            _contexto.SaveChanges();

            return Ok(parqueo);
        }
    }
}
