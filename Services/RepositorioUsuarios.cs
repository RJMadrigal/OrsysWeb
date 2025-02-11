﻿using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.Models;
using System.Diagnostics;
using System.Net;

namespace SistemaOrdenes.Services
{
    public interface IRepositorioUsuarios
    {
        Task EditarUser(EditarUsuarioViewModel usuarios);
        Task<TbUsuario> Obtener(string correo);
        Task<string> ObtenerRolPorId(int id);
        Task<TbUsuario> ObtenerUsuarioPorCredenciales(string correo, string clave);
    }


    public class RepositorioUsuarios: IRepositorioUsuarios
    {
        private readonly DbProyectoAnalisisIiContext context;

        public RepositorioUsuarios(DbProyectoAnalisisIiContext context)
        {
            this.context = context;
        }


        //OBTIENE EL USUARIO POR LA CREDENCIAL
        public async Task<TbUsuario?> ObtenerUsuarioPorCredenciales(string correo, string clave)
        {
            //SE OBTIENE EL USUARIO DE LA BD
            return await context.TbUsuarios
                .FirstOrDefaultAsync(u => u.Correo.ToLower() == correo.ToLower() && u.Clave == clave);
        }


        //EDITAR EL USUARIO
        public async Task EditarUser(EditarUsuarioViewModel usuario)
        {
            //SE BUSCA EL USUARIO POR ID
            var UsuarioEdit = await context.TbUsuarios.FindAsync(usuario.IdUsuario);

            //SE VERIFICA SI ES NULO
            if (UsuarioEdit == null)
            {
                throw new ArgumentNullException(nameof(UsuarioEdit), "Usuario no encontrado.");
            }


            //SE PASA LOS DATOS MEDIANTE ENTITY
            UsuarioEdit.Nombre = usuario.Nombre;
            UsuarioEdit.Usuario = usuario.Usuario;
            UsuarioEdit.Correo = usuario.Correo;
            UsuarioEdit.Restablecer = usuario.Restablecer;
            UsuarioEdit.Confirmado = usuario.Confirmado;
            UsuarioEdit.IdRol = usuario.IdRol;
            UsuarioEdit.IdJefe = usuario.IdJefe;
            UsuarioEdit.Estado = usuario.Estado;

            //SE ACTUALIZA
            context.Update(UsuarioEdit);

            //SE GUARDA LOS CAMBIOS
            await context.SaveChangesAsync();
        }


        //SE OBTIENE EL ROL DEL USUARIO POR ID

        public async Task<string> ObtenerRolPorId(int id)
        {
            //SE OBTIENE EL USUARIO INCLUYENDO SU ROL
            var usuario = await context.TbUsuarios
                .Include(u => u.IdRolNavigation) //Carga la navegación hacia el rol
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            //RETORNA EL NOMBRE DEL ROL O UN MENSAJE DE ERROR
            if (usuario != null && usuario.IdRolNavigation != null)
            {
                return usuario.IdRolNavigation.NombreRol; // Retorna el nombre del rol
            }

            return "Rol no encontrado"; // Manejo si no se encuentra el rol
        }





        //OBTIENE EL USUARIO POR CORREO
        public async Task<TbUsuario?> Obtener(string correo)
        {
            try
            {
                var usuario = await context.TbUsuarios
                   .Where(u => u.Correo == correo)
                   .Select(u => new TbUsuario
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
