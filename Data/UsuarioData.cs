using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.BD;
using SistemaOrdenes.Models;
using System.Data;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaOrdenes.Data
{
    public class UsuarioData
    {
        private readonly DbPruebaOrdenesContext _context;

        public UsuarioData(DbPruebaOrdenesContext context)
        {
            _context = context;
        }

        public async Task<Usuarios?> ObtenerUsuarioPorCredenciales(string Correo, string Clave)
        {
            try
            {
                var usuario = await _context.TbUsuarios.FirstOrDefaultAsync(u => u.Correo == Correo && u.Clave == Clave);
                
                if (usuario != null) 
                {
                    return usuario;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obetener el usuario por credenciales: {ex}");
                throw;
            }
        }



        public async Task EditarUser( int id, Usuarios usuarios)
        {
            var UsuarioEdit = await _context.TbUsuarios.FindAsync(id);

            if (UsuarioEdit == null)
            {
                throw new ArgumentNullException(nameof(UsuarioEdit), "Usuario no encontrado.");
            }

            UsuarioEdit.Nombre = usuarios.Nombre;
            UsuarioEdit.Usuario = usuarios.Usuario;
            UsuarioEdit.Correo = usuarios.Correo;
            UsuarioEdit.Restablecer = usuarios.Restablecer;
            UsuarioEdit.Confirmado = usuarios.Confirmado;
            UsuarioEdit.IdRol = usuarios.IdRol;
            UsuarioEdit.IdJefe = usuarios.IdJefe;

            _context.Update(UsuarioEdit);
            await _context.SaveChangesAsync();
        }


        public async Task<Usuarios> Obtener(string correo)
        {
            try
            {
                var usuario = await _context.TbUsuarios
                   .Where(u => u.Correo == correo)
                   .Select(u => new Usuarios
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
                throw ;
            }  
        }


    }
}
