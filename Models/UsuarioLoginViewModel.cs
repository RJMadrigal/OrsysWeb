using System.ComponentModel.DataAnnotations;

namespace SistemaOrdenes.Models
{
    public class UsuarioLoginViewModel
    {

        [Required(ErrorMessage = "{0} es requerido")]
        public string Correo {  get; set; }


        [Required(ErrorMessage = "{0} es requerido")]
        public string Clave { get; set; }

    }
}
