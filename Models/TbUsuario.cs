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

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public bool? Restablecer { get; set; }

    public bool? Confirmado { get; set; }

    public string? Token { get; set; }


    public int IdRol { get; set; }

    public int? IdJefe { get; set; }

    public virtual TbUsuario? IdJefeNavigation { get; set; }

    public virtual TbRole IdRolNavigation { get; set; } = null!;

    public virtual ICollection<TbUsuario> InverseIdJefeNavigation { get; set; } = new List<TbUsuario>();

    public virtual ICollection<TbHistorial> TbHistorials { get; set; } = new List<TbHistorial>();

    public virtual ICollection<TbOrden> TbOrdens { get; set; } = new List<TbOrden>();
}
