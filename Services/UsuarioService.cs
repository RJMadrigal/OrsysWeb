﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.Models;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Claims;

namespace SistemaOrdenes.Services
{
    public class UsuarioService
    {
        private readonly DbProyectoAnalisisIiContext context;
        private readonly HttpContext httpContext;

        public UsuarioService(DbProyectoAnalisisIiContext context, IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
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



        //OBTIENE EL ID DEL USUARIO AUTENTICADO
        public int ObtenerUsuarioId()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);
                return id;

            }
            else
            {
                throw new Exception("El usuario no está autenticado");
            }

        }

        public async Task<TbUsuario> ObtenerDatosUserOrden(int id)
        {
            int IdComprador = await context.TbOrdens
                .Where(x => x.IdOrden == id)
                .Select(x => x.IdUsuarioComprador)
                .FirstOrDefaultAsync();

            if (IdComprador == 0)
                throw new Exception("No se encontró el usuario comprador actual.");

            var usuarioComprador = await context.TbUsuarios
                .Where(u => u.IdUsuario == IdComprador)
                .FirstOrDefaultAsync();

            return usuarioComprador;
        }

        public async Task<TbUsuario> ObtenerDatosUser()
        {
            int id = ObtenerUsuarioId();

            var usuario = await context.TbUsuarios
                .Where(x => x.IdUsuario == id)
                .FirstOrDefaultAsync();

            if (usuario == null)
                throw new Exception("No se encontró el usuario actual.");

            return usuario;
        }

        public async Task<TbUsuario> ObtenerDatosUserPorID(int id)
        {

            var usuario = await context.TbUsuarios
                .Where(x => x.IdUsuario == id)
                .FirstOrDefaultAsync();

            if (usuario == null)
                throw new Exception("No se encontró el usuario actual.");

            return usuario;
        }




        //OBTIENE LOS DATOS DEL JEFE DEL USUARIO
        public async Task<TbUsuario> ObtenerDatosJefe()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (idClaim == null)
                    throw new Exception("No se encontró el identificador de usuario en los reclamos.");

                var id = int.Parse(idClaim.Value);

                // OBTIENE TODOS LOS DATOS DEL USUARIO JEFE
                var jefe = await context.TbUsuarios
                    .Where(u => u.IdUsuario == id)
                    .Select(u => u.IdJefe)
                    .Join(context.TbUsuarios, idJefe => idJefe,
                          jefeUsuario => jefeUsuario.IdUsuario,
                          (idJefe, jefeUsuario) => jefeUsuario)
                    .FirstOrDefaultAsync();

                if (jefe == null)
                    throw new Exception("No se encontró el jefe para el usuario actual.");

                return jefe;
            }
            else
            {
                throw new Exception("El usuario no está autenticado");
            }
        }



        //OBTIENE EL ID DEL JEFE DEL USUARIO LOGUEADO
        public async Task<int> ObtenerIdJefe()
        {

            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);

                //OBTIENE EL ID DEL USUARIO JEFE
                var idJefe = await context.TbUsuarios.Where(x => x.IdUsuario == id).Select(x => x.IdJefe).FirstOrDefaultAsync();


                //RETORNA EL CORREO
                return int.Parse(idJefe.ToString());

            }
            else
            {
                throw new Exception("El usuario no está autenticado");
            }
        }



        //OBTIENE EL NOMBRE DEL JEFE DEL USUARIO x ID
        public async Task<string> ObtenerNombreUsuario(int id)
        {
            var NombreJefe = await context.TbUsuarios.Where(x => x.IdUsuario == id).Select(x => x.Nombre).FirstOrDefaultAsync();

            return NombreJefe;
        }



        //OBTENER EL NOMBRE DEL JEFE FINANCIERO POR ID DE LA ORDEN
        public async Task<string> ObtenerJefeFinanciero(int idOrden)
        {
            var NombreJefeFinanciero = await context.TbHistorials
                .Where(x => x.IdOrden == idOrden && x.IdUsuarioNavigation.IdRolNavigation.NombreRol.StartsWith("Jefe aprob"))
                .Select(h => h.IdUsuarioNavigation.Nombre).FirstOrDefaultAsync();

            return NombreJefeFinanciero;
        }


        //OBTIENE EL NOMBRE DEL JEFE DEL USUARIO QUE APROBÓ LA ORDEN
        public async Task<string> ObtenerNombreUsuarioAprobador(int idOrden)
        {
            //BUSCA EL NOMBRE DEL JEFE QUE APROBÓ RECHAZO LA ORDEN
            var NombreJefeFinanciero = await context.TbOrdens.Where(x => x.IdOrden == idOrden)
                .Select(x => x.IdUsuarioCompradorNavigation.IdJefeNavigation.Nombre).FirstOrDefaultAsync();

            return NombreJefeFinanciero;
        }
    }
}
