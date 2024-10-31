using SMXapiRESFULL.Entities;

namespace SMXapiRESFULL.Services
{
    public class UsuarioService
    {
        private static List<Usuario> usuarios = [];

        public List<Usuario> FiltrarUsuariosPorDominio(string dominio)
        {
            return usuarios.Where(u => u.CorreoElectronico.EndsWith($"@{dominio}")).ToList();
        }

        public List<Usuario> GetUsuarios()
        {
            return usuarios;
        }

        public Usuario GetUsuario(int id)
        {
            return usuarios.FirstOrDefault(u => u.Id == id);
        }

        public bool IsEmailInUse(string email)
        {
            return usuarios.Any(u => u.CorreoElectronico == email);
        }

        public bool IsIdInUse(int id)
        {
            return usuarios.Any(u => u.Id == id);
        }

        public Usuario AddUsuario(Usuario usuario)
        {
            usuario.Id = usuarios.Count > 0 ? usuarios.Max(u => u.Id) + 1 : 1;
            usuario.FechaRegistro = DateTime.Now;
            usuarios.Add(usuario);
            return usuario;
        }

        public void DeleteUsuario(int id)
        {
            Usuario usuario = usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario != null)
            {
                usuarios.Remove(usuario);
            }
        }
    }
}
