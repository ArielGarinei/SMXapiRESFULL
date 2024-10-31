using Microsoft.AspNetCore.Mvc;
using SMXapiRESFULL.Dtos;
using SMXapiRESFULL.Entities;
using SMXapiRESFULL.Services;
using System.Text.RegularExpressions;

namespace SMXapiRESFULL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("filtrarPorDominio")]
        public ActionResult<IEnumerable<Usuario>> FiltrarUsuariosPorDominio([FromQuery] string dominio)
        {
            List<Usuario> usuariosFiltrados = _usuarioService.FiltrarUsuariosPorDominio(dominio);
            return Ok(usuariosFiltrados);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetUsuarios()
        {
            return Ok(_usuarioService.GetUsuarios());
        }

        [HttpGet("{id}")]
        public ActionResult<Usuario> GetUsuario(int id)
        {
            Usuario usuario = _usuarioService.GetUsuario(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public ActionResult<Usuario> PostUsuario([FromBody] Usuario usuario)
        {
            if (!IsValidEmail(usuario.CorreoElectronico))
            {
                return BadRequest("Correo electrónico no válido.");
            }
            if (_usuarioService.IsEmailInUse(usuario.CorreoElectronico))
            {
                return Conflict("El correo electrónico ya está en uso.");
            }
            if (_usuarioService.IsIdInUse(usuario.Id))
            {
                return Conflict("El ID ya está en uso.");
            }
            Usuario nuevoUsuario = _usuarioService.AddUsuario(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = nuevoUsuario.Id }, nuevoUsuario);
        }

        [HttpPut("{id}")]
        public ActionResult PutUsuario(int id, [FromBody] UsuarioUpdateDto usuarioDto)
        {
            if (!IsValidEmail(usuarioDto.CorreoElectronico))
            {
                return BadRequest("Correo electrónico no válido.");
            }
            Usuario existingUsuario = _usuarioService.GetUsuario(id);
            if (existingUsuario == null)
            {
                return NotFound();
            }
            existingUsuario.Nombre = usuarioDto.Nombre;
            existingUsuario.CorreoElectronico = usuarioDto.CorreoElectronico;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUsuario(int id)
        {
            Usuario usuario = _usuarioService.GetUsuario(id);
            if (usuario == null)
            {
                return NotFound();
            }
            _usuarioService.DeleteUsuario(id);
            return NoContent();
        }

        private bool IsValidEmail(string email)
        {
            Regex emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
    }
}
