namespace SistemaOrdenes.Models
{
    public class InfoOrdenViewModel
    {

        public int IdOrden { get; set; }

        public string NombreArticulo { get; set; } = null!;

        public string Modelo { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }

        public decimal? Total { get; set; }

        public string Estado { get; set; }

        public string JefeAprobador { get; set; }
        public string JefeFinanciero { get; set; }



    }
}
