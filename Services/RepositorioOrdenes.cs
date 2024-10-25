using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.Models;

namespace SistemaOrdenes.Services
{

    public interface IRepositorioOrdenes
    {
        Task<bool> CrearOrden(CrearOrdenViewModel modelo);
        Task<List<OrdenesViewModel>> ObtenerOrdenesComprador(int idUsuario);
        Task<InfoOrdenViewModel> ObtenerOrdenPorId(int id, string NombreJefe, string NombreJefeFinanciero);
    }



    public class RepositorioOrdenes: IRepositorioOrdenes
    {

        private readonly DbProyectoAnalisisIiContext context;

        public RepositorioOrdenes(DbProyectoAnalisisIiContext context)
        {
            this.context = context;
        }



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



        //OBTIENE LA LISTA DE ORDENES DEL USUARIO
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
    }
}
