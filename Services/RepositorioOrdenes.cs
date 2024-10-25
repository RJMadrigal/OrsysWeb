using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.Models;

namespace SistemaOrdenes.Services
{

    public interface IRepositorioOrdenes
    {
        Task<bool> CrearOrden(CrearOrdenViewModel modelo);
        Task<bool> EditarOrdenJefe(int idOrden, string estado, string comentarios, int idJefe);
        Task<List<OrdenesViewModel>> ObtenerOrdenesComprador(int idUsuario);
        Task<List<OrdenesViewModel>> ObtenerOrdenesJefe(int idUsuarioJefe);
        Task<InfoOrdenViewModel> ObtenerOrdenPorId(int id, string NombreJefe, string NombreJefeFinanciero);
        Task<InfoOrdenViewModel> ObtenerOrdenPorId(int idOrden);
    }



    public class RepositorioOrdenes: IRepositorioOrdenes
    {

        private readonly DbProyectoAnalisisIiContext context;

        public RepositorioOrdenes(DbProyectoAnalisisIiContext context)
        {
            this.context = context;
        }



        //CREA LA ORDEN MEDIANTE UN PROCEDIMIENTO ALMACENADO
        public async Task<bool> CrearOrden(CrearOrdenViewModel modelo)
        {
            //TRY PARA EL MANEJO DE ERRORES EN LA BASE DE DATOS
            try
            {
                await context.Database.ExecuteSqlInterpolatedAsync($@"
                            EXEC sp_CrearOrden
                                    @Nombre_Articulo = {modelo.NombreArticulo},
                                    @Modelo = {modelo.Modelo},
                                    @Precio = {modelo.Precio},
                                    @Cantidad = {modelo.Cantidad},
                                    @Detalles = {modelo.Detalles},
                                    @ID_UsuarioComprador = {modelo.idUsuario}");
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }


        //ACTUALIZA EL ESTADO DE LA ORDEN MEDIANTE UN PROCEDIMIENTO ALMACENADO
        public async Task<bool> EditarOrdenJefe(int idOrden, string estado, string comentarios, int idJefe)
        {
            //TRY PARA EL MANEJO DE ERRORES EN LA BASE DE DATOS
            try
            {
                await context.Database.ExecuteSqlInterpolatedAsync($@"
                            EXEC sp_EstadoOrdenJefe
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




        //OBTIENE LA LISTA DE ORDENES DEL USUARIO JEFE
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
                    JefeFinanciero = NombreJefeFinanciero
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
