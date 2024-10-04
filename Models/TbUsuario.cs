using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaOrdenes.Models
{
    public partial class Usuarios
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        public string Usuario { get; set; } = null!;

        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no es válido.")]
        public string Correo { get; set; } = null!;

        
        public string? Clave { get; set; }

        public bool? Restablecer { get; set; }

        public bool? Confirmado { get; set; }

        public string? Token { get; set; }

        [Required(ErrorMessage = "El campo IdRol es obligatorio.")]
        public int IdRol { get; set; }

        public int? IdJefe { get; set; }

        public virtual Usuarios? IdJefeNavigation { get; set; }

        public virtual TbRole? IdRolNavigation { get; set; } = null!;

        public virtual ICollection<Usuarios> InverseIdJefeNavigation { get; set; } = new List<Usuarios>();

        public virtual ICollection<TbOrden> TbOrdens { get; set; } = new List<TbOrden>();
    }
}
