namespace SistemaOrdenes.Models
{
    public class OrdenesViewModel
    {
        public int IdOrden { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string NombreArticulo { get; set; } = null!;

        public string Modelo { get; set; } = null!;

        public decimal? Total { get; set; }


        
    }
}
