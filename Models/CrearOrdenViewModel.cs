using System.ComponentModel.DataAnnotations;

namespace SistemaOrdenes.Models
{
    public class CrearOrdenViewModel
    {
        [Required (ErrorMessage = "Debes ingresar el nombre del artículo")]
        public string NombreArticulo { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar el modelo del artículo")]
        public string Modelo { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar el precio del artículo")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Debes ingresar la cantidad a comprar del artículo")]
        [Range(1, int.MaxValue, ErrorMessage = "El monto debe ser mayor que 0.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Debes ingresar detalles adicionales")]
        public string Detalles { get; set; } = null!;

        public int idUsuario { get; set; }
    }
}
