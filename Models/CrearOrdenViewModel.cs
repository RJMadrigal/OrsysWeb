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
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Debes ingresar la cantidad a comprar del artículo")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Debes ingresar detalles adicionales")]
        public string Detalles { get; set; } = null!;

        public int idUsuario { get; set; }
    }
}
