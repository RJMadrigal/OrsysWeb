using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.Data;
using SistemaOrdenes.Models;
using System.Diagnostics;

namespace SistemaOrdenes.Services
{
    public class UsuarioService
    {
        private readonly DbPruebaOrdenesContext _context;
        public UsuarioService( DbPruebaOrdenesContext context)
        {
            
            _context = context;

        }
        public IEnumerable<Usuarios> ObtenerJefes()
        {
            return _context.TbUsuarios.Where(u => u.IdRol == 2).ToList();
        }
        public IEnumerable<TbRole> ObtenerRoles()
        {
            return _context.TbRoles.ToList();
        }

        public async Task<bool> RegistrarUsuario(Usuarios usuario)
        {
            if (usuario != null)
            {
                usuario.Restablecer = false;
                usuario.Confirmado = false;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
           
        }

        public async Task<bool> RestablecerActualizarAsync(bool restablecer, string clave, string token)
        {
            bool respuesta = false;
            try
            {
                var usuario = await _context.TbUsuarios.FirstOrDefaultAsync(u => u.Token == token);
                if (usuario != null)
                {
                    usuario.Restablecer = restablecer;
                    usuario.Clave = clave;

                    _context.TbUsuarios.Update(usuario);
                    int filasAfectadas = await _context.SaveChangesAsync();
                     
                    if (filasAfectadas > 0)
                        respuesta = true;

                    return respuesta;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al actualizar el usuario: {ex}");
                throw;
            }
        }

        public async Task<Usuarios?> ConfirmarAsync(string token)
        {
            try
            { 
                var usuario = await _context.TbUsuarios.FirstOrDefaultAsync(u => u.Token == token);
                if (usuario != null)
                {
                    return usuario;
                }    
                    return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al consultar cuenta: {ex}");
                throw;
            }
        }


        public async  Task<bool> EstablecerContraseñaActivacion(string token, string clave)
        {
            try
            {
                var usuario = _context.TbUsuarios.FirstOrDefault(u => u.Token == token);
                if (usuario != null )
                {
                    if (usuario.Confirmado == true)
                    {
                        return false;
                    }
                    usuario.Clave = clave;
                    usuario.Confirmado = true;
                    await _context.SaveChangesAsync();
                    return true;
                }       
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al confirmar usuario: {ex}");
                throw ex;
            }
        }



    }
}
