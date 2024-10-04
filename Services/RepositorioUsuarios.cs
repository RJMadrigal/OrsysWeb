using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.BD;
using SistemaOrdenes.Entidades;
using SistemaOrdenes.Models;
using System.Diagnostics;
using System.Net;

namespace SistemaOrdenes.Services
{
    public interface IRepositorioUsuarios
    {
        Task EditarUser(int id, Usuario usuarios);
        Task<Usuario?> Obtener(string correo);
        Task<Usuario> ObtenerUsuarioPorCredenciales(string correo, string clave);
    }


    public class RepositorioUsuarios: IRepositorioUsuarios
    {
        private readonly DbProyectoAnalisisIiContext context;

        public RepositorioUsuarios(DbProyectoAnalisisIiContext context)
        {
            this.context = context;
        }


        //OBTIENE EL USUARIO POR LA CREDENCIAL
        public async Task<Usuario> ObtenerUsuarioPorCredenciales(string correo, string clave)
        {
            //SE OBTIENE EL USUARIO DE LA BD
            var usuario = await context.TbUsuarios
                .FirstOrDefaultAsync(u => u.Correo.ToLower() == correo.ToLower() && u.Clave == clave);


            //SE MAPEA
            return new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                NombreUsuario = usuario.Usuario,
                Correo = usuario.Correo,
                Clave = usuario.Clave,
                Restablecer = usuario.Restablecer,
                Confirmado = usuario.Confirmado,
                Token = usuario.Token,
                IdRol = usuario.IdRol,
                IdJefe = usuario.IdJefe
            };
        }


        //EDITAR EL USUARIO
        public async Task EditarUser(int id, Usuario usuarios)
        {
            //SE BUSCA EL USUARIO POR ID
            var UsuarioEdit = await context.TbUsuarios.FindAsync(id);

            if (UsuarioEdit == null)
            {
                throw new ArgumentNullException(nameof(UsuarioEdit), "Usuario no encontrado.");
            }

            UsuarioEdit.Nombre = usuarios.Nombre;
            UsuarioEdit.Usuario = usuarios.NombreUsuario;
            UsuarioEdit.Correo = usuarios.Correo;
            UsuarioEdit.Restablecer = usuarios.Restablecer;
            UsuarioEdit.Confirmado = usuarios.Confirmado;
            UsuarioEdit.IdRol = usuarios.IdRol;
            UsuarioEdit.IdJefe = usuarios.IdJefe;

            //SE ACTUALIZA
            context.Update(UsuarioEdit);

            //SE GUARDA LOS CAMBIOS
            await context.SaveChangesAsync();
        }



        //OBTIENE EL USUARIO POR CORREO
        public async Task<Usuario?> Obtener(string correo)
        {
            try
            {
                var usuario = await context.TbUsuarios
                   .Where(u => u.Correo == correo)
                   .Select(u => new Usuario
                   {
                       Nombre = u.Nombre,
                       Clave = u.Clave,
                       Restablecer = u.Restablecer,
                       Confirmado = u.Confirmado,
                       Token = u.Token
                   })
                   .FirstOrDefaultAsync();

                return usuario;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener el usuario: {ex}");
                throw;
            }
        }
    }
}
