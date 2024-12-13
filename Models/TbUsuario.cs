using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaOrdenes.Models;

public partial class TbUsuario
{
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Usuario { get; set; } = null!;



    [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Correo { get; set; } = null!;



    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "La contraseña debe tener al menos una letra, un número y un mínimo de 8 caracteres.")]
    public string Clave { get; set; } = null!;

    public bool? Restablecer { get; set; }

    public bool? Confirmado { get; set; }

    public string? Token { get; set; }


    public bool? Estado { get; set; }


    public int IdRol { get; set; }

    public int? IdJefe { get; set; }

    public virtual TbUsuario? IdJefeNavigation { get; set; }

    public virtual TbRole IdRolNavigation { get; set; } = null!;

    public virtual ICollection<TbUsuario> InverseIdJefeNavigation { get; set; } = new List<TbUsuario>();

    public virtual ICollection<TbHistorial> TbHistorials { get; set; } = new List<TbHistorial>();

    public virtual ICollection<TbOrden> TbOrdens { get; set; } = new List<TbOrden>();
}
