using System.ComponentModel.DataAnnotations;

namespace SistemaOrdenes.Models
{
    public class EditarUsuarioViewModel
    {
        public int IdUsuario { get; set; } 

        public string Nombre { get; set; }

        public string Usuario { get; set; }

        public string Correo { get; set; }

        public bool? Restablecer { get; set; }

        public bool? Confirmado { get; set; }

        public int IdRol { get; set; }

        public int? IdJefe { get; set; }


    }
}
