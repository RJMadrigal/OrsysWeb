using System.ComponentModel.DataAnnotations;

namespace SistemaOrdenes.Models
{
    public class RestablecerViewModel
    {
        [Required(ErrorMessage = "Ingresa tu correo")]
        public string Correo {  get; set; }

    }
}
