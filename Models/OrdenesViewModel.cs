namespace SistemaOrdenes.Models
{
    public class OrdenesViewModel
    {
        public int IdOrden { get; set; }

        public DateTime FechaCreacion { get; set; }

        public decimal? Total { get; set; }

        public string Estado { get; set; }

    }
}
