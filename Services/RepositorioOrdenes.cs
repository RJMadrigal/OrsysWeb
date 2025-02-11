﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.Models;
using System.Data;

namespace SistemaOrdenes.Services
{

    public interface IRepositorioOrdenes
    {
        Task<int?> CrearOrden(CrearOrdenViewModel modelo);
        Task<(bool success, int idJefeFinanciero)> EditarOrdenJefe(int idOrden, string estado, string comentarios, int idJefe);
        Task<bool> EditarOrdenJefeFinanciero(int idOrden, string estado, string comentarios, int idJefe);
        Task<List<OrdenesViewModel>> ObtenerOrdenesComprador(int idUsuario);
        Task<List<OrdenesViewModel>> ObtenerOrdenesJefe(int idUsuarioJefe);
        Task<List<OrdenesViewModel>> ObtenerOrdenesJefeFinanciero(int idUsuarioJefe);
        Task<InfoOrdenViewModel> ObtenerOrdenPorId(int id, string NombreJefe, string NombreJefeFinanciero);
        Task<InfoOrdenViewModel> ObtenerOrdenPorId(int idOrden);
        Task<List<OrdenesViewModel>> ObtenerTodasOrdenesJefes(int idUsuarioJefe);
    }



    public class RepositorioOrdenes: IRepositorioOrdenes
    {

        private readonly DbProyectoAnalisisIiContext context;

        public RepositorioOrdenes(DbProyectoAnalisisIiContext context)
        {
            this.context = context;
        }



        //CREA LA ORDEN MEDIANTE UN PROCEDIMIENTO ALMACENADO
        public async Task<int?> CrearOrden(CrearOrdenViewModel modelo)
        {
            try
            {
                // Define un parámetro de salida para capturar el ID de la orden
                var idOrdenParam = new SqlParameter("@IdOrden", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Ejecuta el procedimiento almacenado
                await context.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC sp_CrearOrden
                @Nombre_Articulo = {modelo.NombreArticulo},
                @Modelo = {modelo.Modelo},
                @Precio = {modelo.Precio},
                @Cantidad = {modelo.Cantidad},
                @Detalles = {modelo.Detalles},
                @ID_UsuarioComprador = {modelo.idUsuario},
                @IdOrden = {idOrdenParam} OUTPUT");

                // Retorna el ID de la orden
                return (int?)idOrdenParam.Value;  // Devuelve el ID de la orden
            }
            catch (Exception)
            {
                // Maneja errores y devuelve null en caso de error
                return null;
            }
        }





        //ACTUALIZA EL ESTADO DE LA ORDEN EL JEFE DEL EMPLEADO MEDIANTE UN PROCEDIMIENTO ALMACENADO
        public async Task<(bool success, int idJefeFinanciero)> EditarOrdenJefe(int idOrden, string estado, string comentarios, int idJefe)
        {
            try
            {
                var paramIdJefeFinanciero = new SqlParameter("@idJefeFinanciero", SqlDbType.Int) { Direction = ParameterDirection.Output };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_EstadoOrdenJefe @ID_Orden, @ID_JEFE, @Estado, @Comentarios, @idJefeFinanciero OUTPUT",
                    new SqlParameter("@ID_Orden", idOrden),
                    new SqlParameter("@ID_JEFE", idJefe),
                    new SqlParameter("@Estado", estado),
                    new SqlParameter("@Comentarios", comentarios),
                    paramIdJefeFinanciero);

                return (true, (int)paramIdJefeFinanciero.Value);
            }
            catch
            {
                return (false, 0);
            }
        }


        //ACTUALIZA EL ESTADO DE LA ORDEN EL JEFE APROBADOR MEDIANTE UN PROCEDIMIENTO ALMACENADO
        public async Task<bool> EditarOrdenJefeFinanciero(int idOrden, string estado, string comentarios, int idJefe)
        {
            //TRY PARA EL MANEJO DE ERRORES EN LA BASE DE DATOS
            try
            {
                await context.Database.ExecuteSqlInterpolatedAsync($@"
                            EXEC sp_EstadoOrdenJefeFinanciero
                                    @ID_Orden = {idOrden},
                                    @ID_JEFE = {idJefe},
                                    @Estado = {estado},
                                    @Comentarios = {comentarios}");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        //OBTIENE LA LISTA DE ORDENES DEL USUARIO COMPRADOR
        public async Task<List<OrdenesViewModel>> ObtenerOrdenesComprador(int idUsuario)
        {

            //SELECCIONA LA LISTA DE ORDENES POR ID
            return await context.TbOrdens
            .Where(x=> x.IdUsuarioComprador == idUsuario)
            .Select(x => new OrdenesViewModel
            {
                IdOrden = x.IdOrden,
                FechaCreacion = x.FechaCreacion,
                Total = x.Total,
                Estado = x.Estado,
                NombreArticulo = x.NombreArticulo,
                Modelo = x.Modelo,
            }).ToListAsync();
        }




        //OBTIENE LA LISTA DE ORDENES PENDIENTES DEL USUARIO JEFE
        public async Task<List<OrdenesViewModel>> ObtenerOrdenesJefe(int idUsuarioJefe)
        {

            //SELECCIONA LA LISTA DE ORDENES POR ID
            var ordenes = await context.TbOrdens
            .Where(x => x.IdUsuarioCompradorNavigation.IdJefe == idUsuarioJefe && x.Estado == "Pendiente")
            .Select(x => new OrdenesViewModel
            {
                IdOrden = x.IdOrden,
                NombreArticulo = x.NombreArticulo,
                Modelo = x.Modelo,
                FechaCreacion = x.FechaCreacion,
                Total = x.Total,
                Solicitante = x.IdUsuarioCompradorNavigation.Nombre,
                Estado = x.Estado
            }).ToListAsync();

            return ordenes;
        }

        //OBTIENE LA LISTA DE ORDENES PENDIENTES DEL USUARIO JEFE APROBADOR
        public async Task<List<OrdenesViewModel>> ObtenerOrdenesJefeFinanciero(int idUsuarioJefe)
        {

            //SELECCIONA LA LISTA DE ORDENES POR ID
            var ordenes = await context.TbOrdens
                .Include(o => o.TbHistorials) // Incluye la relación con TbHistorial
                .Where(o => o.TbHistorials.Any(h => h.IdUsuario == idUsuarioJefe && h.Estado == "Pendiente aprobacion"))
                .Select(o => new OrdenesViewModel
                {
                    IdOrden = o.IdOrden,
                    NombreArticulo = o.NombreArticulo,
                    Modelo = o.Modelo,
                    FechaCreacion = o.FechaCreacion,
                    Solicitante = o.IdUsuarioCompradorNavigation.Nombre,
                    Total = o.Total,
                    Estado = o.TbHistorials
                        .Where(h => h.IdUsuario == idUsuarioJefe && h.Estado == "Pendiente aprobacion")
                        .Select(h => h.Estado)
                        .FirstOrDefault() // Selecciona el estado pendiente de aprobación del historial de usuario 31
                }).ToListAsync();

            return ordenes;
        }



        //OBTIENE LA LISTA DE ORDENES PENDIENTES DEL USUARIO JEFE
        public async Task<List<OrdenesViewModel>> ObtenerTodasOrdenesJefes(int idUsuarioJefe)
        {

            //SELECCIONA LA LISTA DE ORDENES POR ID
            var ordenes = await context.TbOrdens
            .Include(o => o.IdUsuarioCompradorNavigation)
            .Include(o => o.TbHistorials)
            .Where(x => x.TbHistorials.Any(h => h.IdUsuario == idUsuarioJefe))
            .Select(x => new OrdenesViewModel
            {
                IdOrden = x.IdOrden,
                NombreArticulo = x.NombreArticulo,
                Modelo = x.Modelo,
                FechaCreacion = x.FechaCreacion,
                Estado = x.Estado,
                Solicitante = x.IdUsuarioCompradorNavigation.Nombre
            }).ToListAsync();

            return ordenes;
        }


        //OBTENER LA ORDEN POR ID
        public async Task<InfoOrdenViewModel> ObtenerOrdenPorId(int id, string NombreJefe, string NombreJefeFinanciero)
        {
            return await context.TbOrdens.Where(x => x.IdOrden == id)
                .Select(x => new InfoOrdenViewModel
                {
                    IdOrden = x.IdOrden,
                    NombreArticulo = x.NombreArticulo,
                    Modelo = x.Modelo,
                    FechaCreacion = x.FechaCreacion,
                    Total = x.Total,
                    Estado = x.Estado,
                    JefeAprobador = NombreJefe,
                    JefeFinanciero = NombreJefeFinanciero,
                    Comentarios = x.TbHistorials.OrderByDescending(h => h.IdHistorial).Select(h => h.Comentarios).FirstOrDefault()
                }).FirstOrDefaultAsync();
        }


        //OBTIENE LA ORDEN SELECCIONADA POR EL JEFE PARA APROBAR O RECHAZAR, SE OBTIENE POR ID
        public async Task<InfoOrdenViewModel> ObtenerOrdenPorId(int idOrden)
        {
            return await context.TbOrdens.Where(x => x.IdOrden == idOrden)
                .Select(x => new InfoOrdenViewModel
                {
                    IdOrden = x.IdOrden,
                    NombreArticulo = x.NombreArticulo,
                    Modelo = x.Modelo,
                    Cantidad = x.Cantidad,
                    Total = x.Total,
                    Detalles = x.Detalles,
                    Solicitante = x.IdUsuarioCompradorNavigation.Nombre
                }).FirstOrDefaultAsync();
        }



       
    }
}
