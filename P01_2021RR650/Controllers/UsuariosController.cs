using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2021RR650.Models;

namespace P01_2021RR650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AplicacionDBContext _contexto;

        public UsuariosController(AplicacionDBContext contexto)
        {
            _contexto = contexto;
        }

        [HttpPost]
        [Route("/CrearUsuario")]
        public IActionResult CrearUsuario([FromBody]Usuario usuario)
        {
            try
            {
                _contexto.Usuarios.Add(usuario);
                _contexto.SaveChanges();
                return Ok(usuario);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/IniciarSesion")]
        public IActionResult IniciarSesion(string nombreUsuario, string contrasena)
        {
            Usuario? usuario = (from e in _contexto.Usuarios where e.usuario == nombreUsuario && e.contrasena == contrasena select e).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound("Credenciales Invalidas.");
            }

            return Ok("Credenciales Validas.");
        }

        [HttpGet]
        [Route("/Usuarios")]
        public IActionResult Usuarios()
        {
            List<Usuario> usuarios = (from e in _contexto.Usuarios select e).ToList();

            return Ok(usuarios);
        }

        [HttpGet]
        [Route("/UsuariosPorRol")]
        public IActionResult UsuariosPorRol(string rol)
        {
            List<Usuario> usuarios = (from e in _contexto.Usuarios where e.rol == rol select e).ToList();

            return Ok(usuarios);
        }

        [HttpGet]
        [Route("/UsuarioPorId")]
        public IActionResult UsuarioPorId(int usuarioId)
        {
            Usuario? usuario = (from e in _contexto.Usuarios where e.usuarioId == usuarioId select e).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound($"Usuario con id: {usuarioId} no encontrado.");
            }

            return Ok(usuario);
        }

        [HttpGet]
        [Route("/UsuariosPorNombre")]
        public IActionResult UsuariosPorNombre(string nombre)
        {
            List<Usuario> usuarios = (from e in _contexto.Usuarios where e.nombre.Contains(nombre) select e).ToList();

            return Ok(usuarios);
        }

        [HttpPut]
        [Route("/ModificaUsuario")]
        public IActionResult ModificaUsuario(int usuarioId, [FromBody] Usuario usuarioModificado)
        {
            Usuario? usuario = (from e in _contexto.Usuarios where e.usuarioId == usuarioId select e).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound($"Usuario con id: {usuarioId} no encontrado.");
            }

            usuario.nombre = usuarioModificado.nombre;
            usuario.correo = usuarioModificado.correo;
            usuario.telefono = usuarioModificado.telefono;
            usuario.usuario = usuarioModificado.usuario;
            usuario.contrasena = usuarioModificado.contrasena;

            _contexto.Usuarios.Entry(usuario).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(usuario);
        }

        [HttpDelete]
        [Route("/EliminarUsuario")]
        public IActionResult EliminarUsuario(int usuarioId)
        {
            Usuario? usuario = (from e in _contexto.Usuarios where e.usuarioId == usuarioId select e).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound($"Usuario con id: {usuarioId} no encontrado.");
            }

            _contexto.Usuarios.Attach(usuario);
            _contexto.Usuarios.Remove(usuario);
            _contexto.SaveChanges();

            return Ok(usuario);
        }
    }
}
