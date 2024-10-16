using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.Models;
using System.Diagnostics;

namespace SistemaOrdenes.Services
{
    public class UsuarioService
    {
        private readonly DbProyectoAnalisisIiContext context;


        public UsuarioService(DbProyectoAnalisisIiContext context)
        {
            
            this.context = context;

        }

        //RETORNA LA LISTA DE USUARIOS JEFES
        public IEnumerable<TbUsuario> ObtenerJefes()
        {
            return context.TbUsuarios.Where(u => u.IdRolNavigation.NombreRol == "Jefe").ToList();
        }
        public IEnumerable<TbRole> ObtenerRoles()
        {
            return context.TbRoles.ToList();
        }


        //REGISTRA UN USUARIO
        public async Task<bool> RegistrarUsuario(TbUsuario usuario)
        {
            if (usuario != null)
            {
                usuario.Restablecer = false;
                usuario.Confirmado = false;
                usuario.Estado = false;
                context.Add(usuario);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
           
        }



        //RESTABLECE LA CONTRASEÑA DEL USUARIO
        public async Task<bool> RestablecerActualizarAsync(bool restablecer, string clave, string token)
        {
            bool respuesta = false;
            try
            {
                var usuario = await context.TbUsuarios.FirstOrDefaultAsync(u => u.Token == token);
                if (usuario != null)
                {
                    usuario.Restablecer = restablecer;
                    usuario.Clave = clave;
                    usuario.Estado = true;

                    context.TbUsuarios.Update(usuario);
                    int filasAfectadas = await context.SaveChangesAsync();
                     
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

        public async Task<TbUsuario?> ConfirmarAsync(string token)
        {
            try
            { 
                var usuario = await context.TbUsuarios.FirstOrDefaultAsync(u => u.Token == token);
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
                var usuario = context.TbUsuarios.FirstOrDefault(u => u.Token == token);
                if (usuario != null )
                {
                    if (usuario.Confirmado == true)
                    {
                        return false;
                    }
                    usuario.Clave = clave;
                    usuario.Confirmado = true;
                    usuario.Estado = true;
                    await context.SaveChangesAsync();
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
