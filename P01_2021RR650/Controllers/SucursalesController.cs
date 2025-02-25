using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2021RR650.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace P01_2021RR650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalesController : ControllerBase
    {
        private readonly AplicacionDBContext _contexto;

        public SucursalesController(AplicacionDBContext contexto)
        {
            _contexto = contexto;
        }

        [HttpPost]
        [Route("/CrearSucursal")]
        public IActionResult CrearSucursal([FromBody]Sucursal sucursal)
        {
            try
            {
                Usuario? usuario = (from e in _contexto.Usuarios where e.usuarioId == sucursal.usuarioId select e).FirstOrDefault();

                if (usuario == null)
                {
                    return NotFound($"Usuario con id: {sucursal.usuarioId} no encontrado.");
                }

                if(!usuario.rol.Equals("ADMINISTRADOR"))
                {
                    return BadRequest("El administrador de la sucursal teniene que tener el ron de ADMINISTRADOR.");
                }

                _contexto.Sucursales.Add(sucursal);
                _contexto.SaveChanges();
                return Ok(sucursal);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/Sucursales")]
        public IActionResult Sucursales()
        {
            var sucursales = (from s in _contexto.Sucursales
                              join u in _contexto.Usuarios
                              on s.usuarioId equals u.usuarioId
                              select new
                              {
                                  s.sucursalId,
                                  s.nombre,
                                  s.direccion,
                                  s.telefono,
                                  s.cantidadParqueos,
                                  Administrador = u.nombre
                              }).ToList();

            return Ok(sucursales);
        }

        [HttpGet]
        [Route("/SucursalPorId")]
        public IActionResult SucursalPorId(int sucursalId)
        {
            var sucursal = (from s in _contexto.Sucursales
                            join u in _contexto.Usuarios
                            on s.usuarioId equals u.usuarioId
                            where s.sucursalId == sucursalId
                            select new
                            {
                                s.sucursalId,
                                s.nombre,
                                s.direccion,
                                s.telefono,
                                s.cantidadParqueos,
                                Administrador = u.nombre
                            }).FirstOrDefault();

            if (sucursal == null)
            {
                return NotFound($"Sucursal con el id:{sucursalId} no fue encontrada.");
            }

            return Ok(sucursal);
        }

        [HttpGet]
        [Route("/SucursalPorNombre")]
        public IActionResult SucursalPorNombre(string nombre)
        {
            var sucursal = (from s in _contexto.Sucursales 
                            join u in _contexto.Usuarios
                            on s.usuarioId equals u.usuarioId
                            where s.nombre == nombre 
                            select new
                            {
                                s.sucursalId,
                                s.nombre,
                                s.direccion,
                                s.telefono,
                                s.cantidadParqueos,
                                Administrador = u.nombre
                            }).FirstOrDefault();

            if (sucursal == null)
            {
                return NotFound($"Sucursal con el nombre:{nombre} no fue encontrada.");
            }

            return Ok(sucursal);
        }

        [HttpPut]
        [Route("/ModificarSucursal")]
        public IActionResult ModificarSucursal(int sucursalId, [FromBody] Sucursal sucursalModificada)
        {
            Sucursal? sucursal = (from e in _contexto.Sucursales where e.sucursalId == sucursalId select e).FirstOrDefault();

            if(sucursal == null)
            {
                return NotFound($"Sucursal con el id:{sucursalId} no encontrada.");
            }

            sucursal.nombre = sucursalModificada.nombre;
            sucursal.direccion = sucursalModificada.direccion;
            sucursal.telefono = sucursalModificada.telefono;
            sucursal.cantidadParqueos = sucursalModificada.cantidadParqueos;
            sucursal.usuarioId = sucursalModificada.usuarioId;

            _contexto.Sucursales.Entry(sucursal).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(sucursal);
        }

        [HttpDelete]
        [Route("/EliminarSucursal")]
        public IActionResult EliminarSucursal(int sucursalId)
        {
            Sucursal? sucursal = (from e in _contexto.Sucursales where e.sucursalId == sucursalId select e).FirstOrDefault();

            if (sucursal == null)
            {
                return NotFound($"Sucursal con el id:{sucursalId} no encontrada.");
            }

            _contexto.Sucursales.Attach(sucursal);
            _contexto.Sucursales.Remove(sucursal);
            _contexto.SaveChanges();

            return Ok(sucursal);
        }
    }
}
